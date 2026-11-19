#region

using MailKit;

using Microsoft.AspNetCore.Mvc.RazorPages;

using MimeKit;

using SendGrid;
using SendGrid.Helpers.Mail;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Sindvamb.Templates;

#endregion

namespace Sindvamb.Service.Net
{
    public interface ISmtpClientHelper : IDisposable
    {
        string Sender { get; set; }

        MessageSendResponse Send(
            string subject,
            string toEmail,
            TemplateKey templateKey,
            PageModel model,
            CancellationToken cancellationToken = default, ITransferProgress progress = null);

        Task<MessageSendResponse> SendSmtpAsync(
            string subject,
            string toEmail,
            TemplateKey templateKey,
            PageModel model,
            CancellationToken cancellationToken = default, ITransferProgress progress = null, string template = null);

        Task<MessageSendResponse> SendSimpleSmtpAsync(
            string subject,
            string toEmail,
            string template,
            CancellationToken cancellationToken = default, ITransferProgress progress = null);

        Task<Response> SendGridAsync(
            string subject,
            string toEmail,
            string toName,
            TemplateKey templateKey,
            PageModel model,
            CancellationToken cancellationToken = default);

        MimeMessage GetMailKitMessage(string subject, string toName, string toEmail, string html, List<KeyValuePair<string, byte[]>> attachments = null);
        SendGridMessage GetSendGridMessage(string subject, string toName, string toEmail, string html, List<KeyValuePair<string, byte[]>> attachments = null);
        MimeMessage GetMessageLinkedResources(string subject, string toName, string toEmail, string html, List<KeyValuePair<string, byte[]>> linkedResources = null);
    }
}