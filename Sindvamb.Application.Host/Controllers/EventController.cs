using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sindvamb.Application.Core;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.EventSourcing;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Security.Configurations;

namespace Sindvamb.Application.Host.Controllers;


[Authorize(Policy = AuthPolicy.MASTER)]
public class EventController : AppControllerBase
{
    public IMediatorHandler MediatorHandler { get; set; }
    private readonly IEventSourcingRepository _eventSourcingRepository;

    public EventController(INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediatorHandler,
        IEventSourcingRepository eventSourcingRepository) : base(notifications, mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
        _eventSourcingRepository = eventSourcingRepository;
    }

    /// <summary>
    /// Eventos da aplicação.
    /// </summary>
    /// <remarks> Retorna uma lista de eventos de dominios que estão amazenados em um EventStore </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>

    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpGet("{aggregateId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<StoredEvent>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetEvents([Required] Guid aggregateId)
    {
        var eventos = (await _eventSourcingRepository.GetEvents(aggregateId))?.ToList()!;

        if (eventos != null && eventos.Count > 0)
        {
            return Ok(eventos);
        }

        return NoContent();
    }
}
