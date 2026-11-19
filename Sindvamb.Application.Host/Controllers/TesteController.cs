using System.Text;

using JetBrains.Annotations;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using OpenHtmlToPdf;

using Sindvamb.Application.Core;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Files.Pdf;
using Sindvamb.Repository.Dapper;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Service.Net;
using Sindvamb.Templates;
using Sindvamb.Templates.Pages.Usuarios;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;

namespace Sindvamb.Application.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TesteController : AppControllerBase
    {
        public ISindvambRepository<User, Guid> UserRepository { get; set; }
        public ISindvambRepository<FileControl, Guid> FileControlRepository { get; set; }
        public IDapperRepository<User, Guid>? UserDapperRepository { get; set; }
        private ISmtpClientHelper Email { get; set; }


        private readonly ILogger<TesteController> _logger;

        public PdfGenerator PdfGenerator { get; set; }
        public TesteController(
            [NotNull][ItemNotNull] INotificationHandler<DomainNotification> notifications,
            [NotNull] IMediatorHandler mediatorHandler,
            ISindvambRepository<User, Guid> userRepository,
            //IDapperRepository<User, Guid> userDapperRepository,
            ISmtpClientHelper email, ILogger<TesteController> logger,
            PdfGenerator pdfGenerator,
            ISindvambRepository<FileControl, Guid> fileControlRepository) : base(notifications, mediatorHandler)
        {
            UserRepository = userRepository;
            //UserDapperRepository = userDapperRepository;
            Email = email;
            _logger = logger;
            PdfGenerator = pdfGenerator;
            FileControlRepository = fileControlRepository;
        }

        [HttpGet("test-smtp")]
        public async Task<IActionResult> TestSmtp()
        {
            try
            {

                var result1 = await Email.SendSmtpAsync(
                    "Solicitação de Contato - Sindvamb-DF",
                    //"willian__bsb@hotmail.com",
                    "tecnologia@sindvamb.com.br",
                    TemplateKey.REQUESTCONTACTNOTIFY,
                    new RequestContactNotify
                    {
                        Request = new()
                        {
                            Profile = "Perfil Ambulante",
                            Subject = "Solicitar inoformações gerais",
                            Message = " is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electr",
                            Response = "typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                            Contact = new()
                            {
                                Name = "Willian Alves da SIlva",
                                Email = "willian_alves_df@yahoo.com.br",
                                CellPhone = "61985119003"
                            },
                        }
                    }
                );

                //var result1 = await Email.SendSmtpAsync(
                //    "Solicitação de Contato - Sindvamb-DF",
                //    //"willian__bsb@hotmail.com",
                //    "tecnologia@sindvamb.com.br",
                //    TemplateKey.REPLYCONTACTREQUEST,
                //    new ReplyRequestContact
                //    {
                //        ContactRequest = new()
                //        {
                //            Subject = "Solicitar inoformações gerais",
                //            Message = " is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electr",
                //            Response = "typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                //            Contact = new Contact()
                //            {
                //                Name = "Willian Alves da SIlva",
                //                Email = "willian_alves_df@yahoo.com.br",
                //            },
                //        }
                //    }
                //);

                //var result = await Email.SendSmtpAsync(
                //    $"Recuperação de Senha - Sindvamb",
                //    //"willian__bsb@hotmail.com",
                //    "tecnologia@sindvamb.com.br",
                //    TemplateKey.RECOVERYPASSWORD,
                //    new RecoveryPassword
                //    {
                //        Nome = "Willian Alves da SIlva",
                //        Url = $"update-password/{Guid.NewGuid().ToString().GetDigits()}",
                //        FrontEndUrl = "https://localhost:7078"
                //    }
                //);

                //var result = await Email.SendSmtpAsync(
                //    "Bem Vindo ao Sindicato dos Ambulantes DF",
                //    //"willian__bsb@hotmail.com",
                //    "tecnologia@sindvamb.com.br",
                //    TemplateKey.REQUESTCONTACT,
                //    new RequestContact
                //    {
                //        Name = "Willian Alves da SIlva",
                //        Email = "willian_alves_df@yahoo.com.br",
                //    }
                //);


                //var result = await Email.SendSmtpAsync(
                //    "Bem Vindo ao Sindicato dos Ambulantes DF",
                //    //"willian__bsb@hotmail.com",
                //    "tecnologia@sindvamb.com.br",
                //    TemplateKey.REGISTRATIONREQUEST,
                //    new RegistrationRequestEmail
                //    {
                //        Nome = "Willian Alves da SIlva",
                //        Email = "willian_alves_df@yahoo.com.br",
                //        CpfCnpj = "33750529877",
                //        Protocolo = "4535345432024"
                //    }
                //);

                //var result = await Email.SendSmtpAsync(
                //    "Bem Vindo ao Sindicato dos Ambulantes DF",
                //    //"willian__bsb@hotmail.com",
                //    "tecnologia@sindvamb.com.br",
                //    TemplateKey.REGISTRATIONREQUESTAPPROVED,
                //    new RegistrationRequestEmailApproved
                //    {
                //        Nome = "Willian Alves da SIlva",
                //        Email = "willian_alves_df@yahoo.com.br",
                //        CpfCnpj = "33750529877",
                //        Protocolo = "4535345432024"
                //    }
                //);

                //var result1 = await Email.SendSmtpAsync(
                //    "Solicitação de cadastro - Sindvamb-DF",
                //    //"willian__bsb@hotmail.com",
                //    "tecnologia@sindvamb.com.br",
                //    TemplateKey.REGISTRATIONREQUESTNOTIFY,
                //    new RegistrationRequestEmailNotify
                //    {
                //        Nome = "Willian Alves da SIlva",
                //        Email = "willian_alves_df@yahoo.com.br",
                //        CpfCnpj = "33750529877",
                //        Protocolo = "4535345432024"
                //    }
                //);

                //var result2 = await Email.SendSmtpAsync(
                //    "Solicitação de cadastro não aprovada - Sindvamb-DF",
                //    //"willian__bsb@hotmail.com",
                //    "tecnologia@sindvamb.com.br",
                //    TemplateKey.REGISTRATIONREQUESTDENY,
                //    new RegistrationRequestEmailDeny
                //    {
                //        Nome = "Willian Alves da SIlva",
                //        Email = "willian_alves_df@yahoo.com.br",
                //        CpfCnpj = "33750529877",
                //        Protocolo = "4535345432024",
                //        Motivo = "Sua solicitação não foi aprovada no momento pois os documentos estavam ilegíveis, por favor tire copias de melhor qualidade e tente novamente!"
                //    }
                //);
            }
            catch (Exception e)
            {
            }

            //var ctx = new SindvambDbContext(new DbContextOptionsBuilder().Options);
            //return Ok(await ctx.User.AnyAsync());

            //var result = await UserDapperRepository.Session.Connection.QueryAsync<User>("SELECT * FROM \"Sindvamb\".\"User\";");            
            //return Ok();


            //return await Task.FromResult(Ok(UserRepository.Any()));
            return Ok("OK");

            //return Ok(await ctx.GetAll());
        }

        [HttpGet("test-sendgrid")]
        public async Task<IActionResult> TestSendGrid()
        {
            try
            {
                var response = await Email.SendGridAsync(
                    "Bem Vindo ao Sindicato dos Ambulantes DF",
                    "willian__bsb@hotmail.com",
                    "Willian Alves da SIlva",
                    TemplateKey.REGISTRATIONREQUEST,
                    new RegistrationRequestEmail
                    {
                        Nome = "Willian Alves da SIlva",
                        Email = "willian_alves_df@yahoo.com.br",
                        CpfCnpj = "33750529877",
                        Protocolo = "4535345432024"
                    }
                );

                //if (!response.Successful)
                if (!response.IsSuccessStatusCode)
                {
                    var response2 = await response.DeserializeResponseBodyAsync(response?.Body);
                    return UnprocessableEntity(
                        $"Não foi possível enviar o código de ativação para o e-mail willian__bsb@hotmail.com, Erro: {response}.");
                }

                var messageId = response.Headers?.Single(x => x.Key == "X-Message-Id").Value?.FirstOrDefault();
            }
            catch
            {
            }

            return Ok("OK");
        }

        [HttpPost("gerar-declaracoes/{cpfCnpj}")]
        public async Task<IActionResult> TestPdf([FromRoute] string cpfCnpj)
        {
            try
            {
                //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                var cpf = cpfCnpj.GetDigits();

                var user = UserRepository
                    .Where(u => u.CpfCnpj.Contains(cpf))
                    .Include(u => u.Address)
                    .FirstOrDefault();

                //var cpfText = user.CpfCnpj.Length > 11 ? "CNPJ Nº" : "CPF Nº";

                //var htmlAss = await TemplateHelper.GetHtml(TemplateKey.DECLARACAOASSOCIACAO, new DeclaracaoAssociacao
                //{
                //    Name = user.Name,
                //    CpfCnpj = $"{cpfText} {Formaters.FormatCpfCnpj(user.CpfCnpj)}",
                //    RG = $"{user.RG} SSP {user.UFIssuingBody}",
                //    Endereco = $"{user.Address?.AddressLine1} {user.Address?.Number} {user.Address?.Neighborhood} - CEP: {Formaters.FormatCEP(user.Address?.ZipCode)} {user.Address?.City}-{user.Address?.UF}"
                //});

                //var pdf = await PdfGenerator.Create(htmlAss, "Declaracao de Associação", true);

                //await FileControlRepository.AddAsync(new FileControl
                //{
                //    FileName = $"Declaracao de Associacao.pdf",
                //    Description = $"Declaracao de Associacao.pdf",
                //    ContentType = "application/pdf",
                //    FileSize = pdfAss.Length,
                //    FileArray = pdfAss,
                //    UserId = user.Id,
                //}, true);

                var htmlSesc = await TemplateHelper.GetHtml(TemplateKey.DECLARACAOSESC, new DeclaracaoSesc
                {
                    Name = user.Name,
                });

                var pdf = await PdfGenerator.Create(htmlSesc, "Declaracao de Sesc", true);

                //await FileControlRepository.AddAsync(new FileControl
                //{
                //    FileName = $"Declaracao Sesc.pdf",
                //    Description = $"Declaracao Sesc.pdf",
                //    ContentType = "application/pdf",
                //    FileSize = pdfSesc.Length,
                //    FileArray = pdfSesc,
                //    UserId = user.Id,
                //},true);


                return File(pdf, "application/pdf", "Declaracao de Associação.pdf");
            }
            catch (Exception e)
            {
                return BadRequest(e.Serialize());
            }

            return Ok("OK");
        }
    }
}