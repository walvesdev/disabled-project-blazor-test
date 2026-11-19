using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sindvamb.Application.Core;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Inputs;

namespace Sindvamb.Application.Host.Controllers
{
    public class SpecialNeedsController : AppControllerBase
    {
        public IMediatorHandler MediatorHandler { get; }

        public SpecialNeedsController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            MediatorHandler = mediatorHandler;
        }


        /// <summary>
        /// Retorna uma lista de SpecialNeeds.
        /// </summary>
        /// <remarks> Retorna uma lista de SpecialNeeds. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<List<SpecialNeedsDTO>>, List<SpecialNeedsDTO>>(new GenericCommand<List<SpecialNeedsDTO>>()));
        }

        /// <summary>
        /// Retorna um SpecialNeeds.
        /// </summary>
        /// <remarks> Retorna um SpecialNeeds. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<Guid, SpecialNeedsDTO>, SpecialNeedsDTO>(new GenericCommand<Guid, SpecialNeedsDTO>(id)));
        }

        /// <summary>
        /// Cadastro de SpecialNeeds.
        /// </summary>
        /// <remarks> Cadastra um novo SpecialNeeds. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpecialNeedsDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<CreateInput<SpecialNeedsDTO>, bool>, bool>(new GenericCommand<CreateInput<SpecialNeedsDTO>, bool>(new CreateInput<SpecialNeedsDTO>(item))));
        }

        /// <summary>
        /// Atualização de SpecialNeeds.
        /// </summary>
        /// <remarks> Atualização de Partne. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SpecialNeedsDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<UpdateInput<SpecialNeedsDTO>, bool>, bool>(new GenericCommand<UpdateInput<SpecialNeedsDTO>, bool>(new UpdateInput<SpecialNeedsDTO>(item))));
        }

        /// <summary>
        /// Deleta um SpecialNeeds.
        /// </summary>
        /// <remarks> Deleta um SpecialNeeds. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<DeleteInput<Guid, SpecialNeeds>, bool>, bool>(new GenericCommand<DeleteInput<Guid, SpecialNeeds>, bool>(new DeleteInput<Guid, SpecialNeeds>(id))));
        }
    }
}
