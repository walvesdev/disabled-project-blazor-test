using MediatR;
using Microsoft.AspNetCore.Mvc;

using Sindvamb.Application.Core;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Service.Commands.Customers;
using Sindvamb.Service.Commands.FileControls;

namespace Sindvamb.Application.Host.Controllers;

public class FileControlController : AppControllerCrudBase<FileControl, FileControlDTO, Guid>
{
    public FileControlController(
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediatorHandler) : base(notifications,
        mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }

    /// <summary>
    /// Retorna uma lista de arquivos.
    /// </summary>
    /// <remarks> Retornauma lista de arquivos. </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpGet("{userId:Guid}/{isDependent:bool}")]
    public async Task<IActionResult> GetUserFiles(Guid userId, bool isDependent)
    {
        return await OkActionResult(await MediatorHandler.SendCommand<GetUserFilesComand, List<FileControlDTO>>(new GetUserFilesComand(userId, isDependent)));
    }
    
    /// <summary>
    /// Altera o Status de um arquivo.
    /// </summary>
    /// <remarks> Altera o Status de um arquivo. </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpPost]
    public async Task<IActionResult> ApprovedChange(FileControlDTO dto)
    {
        return await OkActionResult(
            await MediatorHandler.SendCommand<ApprovedChangeCommand, bool>(new ApprovedChangeCommand(dto)));
    }

}