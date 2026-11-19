using System.IO;
using log4net;
using log4net.Appender;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sindvamb.Application.Core;
using Microsoft.AspNetCore.Authorization;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;

namespace Sindvamb.Application.Host.Controllers;

// [ApiExplorerSettings(IgnoreApi = true)]
public class ErrorLoggingController : AppControllerBase
{
    public IMediatorHandler MediatorHandler { get; }

    public ErrorLoggingController(
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }


    /// <summary>
    /// Log de Erros da Aplicação
    /// </summary>
    /// <remarks> Retorna um aquivo .log contendo as exceções lançadas na Aplicação </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpGet("{matricula:int}/{code}")]
    [AllowAnonymous]
    public IActionResult Generate(int matricula, string code)
    {
        if (matricula == 337505 && code == "1q2w3E*")
        {
            var path = (LogManager.GetCurrentLoggers()[0].Logger.Repository.GetAppenders()[0] as FileAppender).File;
            var result = new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
                "binary/octet-stream");
            result.FileDownloadName = "Logs.log";
            return result;
        }
        return new ForbidResult();
    }
}
