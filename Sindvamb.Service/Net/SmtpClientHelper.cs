#region

using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sindvamb.Domain.Entities;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Templates;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;

#endregion

namespace Sindvamb.Service.Net
{
    public enum MailProvider
    {
        [EnumMember(Value = "SmtpRelay")] SmtpRelay,
        [EnumMember(Value = "SendGrid")] SendGrid
    }

    public class SmtpClientHelper : ISmtpClientHelper
    {
        public AppSettingsSection AppSettings { get; }
        public SecureSocketOptions SecureOptions { get; set; }
        public ISindvambRepository<EmailHistory, Guid> EmailHistoryRepository { get; set; }
        public string Sender { get; set; }
        private readonly int _timeout;

        public SmtpClientHelper(
            IOptions<AppSettingsSection> optionss,
            ISindvambRepository<EmailHistory, Guid> emailHistoryRepository, string sender = "Sindvamb")
        {
            AppSettings = optionss.Value;
            EmailHistoryRepository = emailHistoryRepository;
            Sender = sender;

            var timeout = TimeSpan.FromMinutes(AppSettings.Mail.TimeoutInMinutes);
            _timeout = (int)timeout.TotalMilliseconds;

            SecureOptions = SecureSocketOptions.Auto;

            if (AppSettings.Mail.EnableSsl)
            {
                SecureOptions = SecureSocketOptions.SslOnConnect;
            }

            if (AppSettings.Mail.EnableTLS)
            {
                SecureOptions = SecureSocketOptions.StartTlsWhenAvailable;
            }
        }


        public MimeMessage GetMailKitMessage(string subject, string toName, string toEmail, string html,
            List<KeyValuePair<string, byte[]>> attachments = null)
        {
            var builder = new BodyBuilder();
            builder.HtmlBody = html;

            attachments?.ForEach(file => { builder.Attachments.Add(file.Key, file.Value); });

            var message = new MimeMessage();
            message.Subject = subject;
            message.To.Add(new MailboxAddress(Encoding.UTF8, toName?.Trim(), toEmail.Trim()));
            message.Body = builder.ToMessageBody();
            return message;
        }

        public SendGridMessage GetSendGridMessage(string subject, string toName, string toEmail, string html,
            List<KeyValuePair<string, byte[]>> attachments = null)
        {
            Sender = Sender ?? "Sindicato dos Ambulantes DF";

            var from = new EmailAddress("contato@sindvamb.com.br", Sender);
            var to = new EmailAddress(toEmail, toName);
            var plainTextContent = html.StripHtml();
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, html);

            attachments?.ForEach(file => { msg.AddAttachment(file.Key, Convert.ToBase64String(file.Value)); });

            //#if DEBUG
            //            msg.MailSettings = new MailSettings
            //            {
            //                SandboxMode = new SandboxMode
            //                {
            //                    Enable = true
            //                }
            //            };
            //#endif

            return msg;
        }

        public MimeMessage GetMessageLinkedResources(
            string subject,
            string toName,
            string toEmail,
            string html,
            List<KeyValuePair<string, byte[]>> linkedResources = null)
        {
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;

            var builder = new BodyBuilder();

            if (linkedResources != null)
            {
                foreach (var (resourceName, bytes) in linkedResources)
                {
                    if (!string.IsNullOrWhiteSpace(resourceName) && bytes != null)
                    {
                        var contentId = $"${resourceName}$";
                        var image = builder.LinkedResources.Add($"{resourceName}.png", bytes);
                        image.ContentId = contentId; // MimeUtils.GenerateMessageId();
                        builder.HtmlBody = html.Replace(contentId, image.ContentId);
                    }
                }
            }
            else
            {
                builder.HtmlBody = html;
            }

            message.Body = builder.ToMessageBody();
            return message;
        }

        public async Task<MessageSendResponse> SendSimpleSmtpAsync(
            string subject,
            string toEmail,
            string template, 
            CancellationToken cancellationToken = default,
            ITransferProgress progress = null)
        {
            MessageSendResponse response = default;
            try
            {

                var emailMessage = GetMailKitMessage(subject, "Suporte", toEmail, template);

                response = await SendPrivateAsync(emailMessage);

                SaveEmailHistory(toEmail, "Exception", default, response, emailMessage.MessageId, false);

            }
            catch
            {
                // ignore
            }


            return response;
        }

        public async Task<MessageSendResponse> SendSmtpAsync(
            string subject,
            string toEmail,
            TemplateKey templateKey,
            PageModel model,
            CancellationToken cancellationToken = default, ITransferProgress progress = null, string template = null)
        {

            try
            {

                template = await TemplateHelper.GetHtml(templateKey, model);

            }
            catch
            {
                // ignore
            }


            var emailMessage = GetMailKitMessage(subject,
                string.Empty,
                toEmail,
                template);

            var response = await SendPrivateAsync(emailMessage);

            var templateName = templateKey.GetDisplayName();

            SaveEmailHistory(toEmail, templateName, model, response, emailMessage.MessageId, false);

            return response;
        }

        public MessageSendResponse Send(
            string subject,
            string toEmail,
            TemplateKey templateKey,
            PageModel model,
            CancellationToken cancellationToken = default, ITransferProgress progress = null)
        {
            var template = TemplateHelper.GetHtml(templateKey, model).Result;

            var emailMessage = GetMailKitMessage(subject,
                string.Empty,
                toEmail,
                template);

            var response = SendPrivate(emailMessage);

            var templateName = templateKey.GetDisplayName();

            SaveEmailHistory(toEmail, templateName, model, response, emailMessage.MessageId, false);

            return response;
        }

        private async void SaveEmailHistory(string toEmail, string templateName, PageModel model, dynamic response,
            string messageId, bool isSenGrid)
        {
            string status;
            bool isSuccess;

            if (!isSenGrid)
            {
                status = JsonConvert.SerializeObject(response.ErrorMessages, Formatting.None);
                isSuccess = response.Successful;

                if (status.Equals("[]"))
                {
                    status = "Enviado";
                }
            }
            else
            {
                isSuccess = response.IsSuccessStatusCode;
                status = await response.DeserializeResponseBodyAsync(response?.Body);
            }

            var history = new EmailHistory
            {
                IpAddress = EmailHistoryRepository.IpAddress,
                Email = toEmail,
                UserId = EmailHistoryRepository.Context.UserId,
                MessageId = messageId,
                IsSuccess = isSuccess,
                Reason = status ?? "Enviado",
                TemplateKey = templateName,         
                Data = templateName,
                Date = DateTime.UtcNow
            };

            if (model.IsNotNull())
            {
                history.Data = model.Serialize(Formatting.None);
            }

            EmailHistoryRepository.Add(history);
            EmailHistoryRepository.SaveChanges();
        }


        public async Task<Response> SendGridAsync(
            string subject,
            string toEmail,
            string toName,
            TemplateKey templateKey,
            PageModel model,
            CancellationToken cancellationToken = default)
        {
            if (!AppSettings.Mail.UseSendGrid)
            {
                return await SendPrivateSmtpAsync(subject, toEmail, templateKey, model, default!, templateKey.GetDisplayName());
            }

            var message = GetSendGridMessage(
                subject,
                toName,
                toEmail,
                await TemplateHelper.GetHtml(templateKey, model)
            );

            var client = new SendGridClient(AppSettings.SendGrid.ApiKey);
            var response = await client.SendEmailAsync(message, cancellationToken);

            var templateName = templateKey.GetDisplayName();
            var messageId = response.Headers?.Single(x => x.Key == "X-Message-Id").Value?.FirstOrDefault();

            if (!response.IsSuccessStatusCode)
            {
                return await SendPrivateSmtpAsync(subject, toEmail, templateKey, model, response, templateName);
            }

            SaveEmailHistory(toEmail, templateName, model, response, messageId, true);

            return response;
        }

        private async Task<Response> SendPrivateSmtpAsync(string subject, string toEmail, TemplateKey templateKey,
            PageModel model,
            Response response = null, string templateName = null)
        {
            _ = await SendSmtpAsync(
                subject,
                toEmail,
                templateKey,
                model
            );

            return response;
        }

        private async Task<MessageSendResponse> SendPrivateAsync(MimeMessage message,
            CancellationToken cancellationToken = default, ITransferProgress progress = null)
        {
            var response = new MessageSendResponse();

            try
            {
                Sender = Sender ?? "Sindvamb";
                message.From.Add(new MailboxAddress(Sender, AppSettings.Mail.Email));

                if (cancellationToken != null && cancellationToken.IsCancellationRequested)
                {
                    response.ErrorMessages.Add("Message was cancelled by cancellation token.");
                    return response;
                }


                using var client = new SmtpClient { Timeout = _timeout };
                await client.ConnectAsync(AppSettings.Mail.SMTPServer, AppSettings.Mail.SMTPPort, SecureOptions,
                    cancellationToken);
                await client.AuthenticateAsync(AppSettings.Mail.Email, AppSettings.Mail.Password, cancellationToken);
                await client.SendAsync(message, cancellationToken, progress);
                await client.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }


            return response;
        }

        private MessageSendResponse SendPrivate(MimeMessage message, CancellationToken cancellationToken = default,
            ITransferProgress progress = null)
        {
            var response = new MessageSendResponse();

            try
            {
                Sender = Sender ?? "Sindvamb";
                message.From.Add(new MailboxAddress(Sender, AppSettings.Mail.Email));

                if (cancellationToken != null && cancellationToken.IsCancellationRequested)
                {
                    response.ErrorMessages.Add("Message was cancelled by cancellation token.");
                    return response;
                }

                using (var client = new SmtpClient { Timeout = _timeout })
                {
                    client.ConnectAsync(AppSettings.Mail.SMTPServer, AppSettings.Mail.SMTPPort, SecureOptions,
                        cancellationToken);
                    client.AuthenticateAsync(AppSettings.Mail.Email, AppSettings.Mail.Password, cancellationToken);
                    client.SendAsync(message, cancellationToken, progress);
                    client.DisconnectAsync(true, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public void Dispose()
        {
        }


    }
}