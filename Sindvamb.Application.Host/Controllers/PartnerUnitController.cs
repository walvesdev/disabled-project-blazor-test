using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sindvamb.Application.Core;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Inputs;

namespace Sindvamb.Application.Host.Controllers
{
    public class PartnerUnitController : AppControllerBase
    {
        public IMediatorHandler MediatorHandler { get; }

        public PartnerUnitController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            ISindvambRepository<User, Guid> repository) : base(notifications, mediatorHandler)
        {
            MediatorHandler = mediatorHandler;
        }

        /// <summary>
        /// Retorna uma lista de Partner.
        /// </summary>
        /// <remarks> Retorna uma lista de Partner. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<List<PartnerUnitDTO>>, List<PartnerUnitDTO>>(new GenericCommand<List<PartnerUnitDTO>>()));
        }


        /// <summary>
        /// Retorna um Partner.
        /// </summary>
        /// <remarks> Retorna um Partner. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<Guid, PartnerUnitDTO>, PartnerUnitDTO>(new GenericCommand<Guid, PartnerUnitDTO>(id)));
        }

        /// <summary>
        /// Cadastro de Partner.
        /// </summary>
        /// <remarks> Cadastra um novo Partner. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PartnerUnitDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<CreateInput<PartnerUnitDTO>, bool>, bool>(new GenericCommand<CreateInput<PartnerUnitDTO>, bool>(new CreateInput<PartnerUnitDTO>(item))));
        }

        /// <summary>
        /// Atualização de Partner.
        /// </summary>
        /// <remarks> Atualização de Partne. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PartnerUnitDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<UpdateInput<PartnerUnitDTO>, bool>, bool>(new GenericCommand<UpdateInput<PartnerUnitDTO>, bool>(new UpdateInput<PartnerUnitDTO>(item))));
        }

        /// <summary>
        /// Deleta um Partner.
        /// </summary>
        /// <remarks> Deleta um Partner. </remarks>
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
                 .SendCommand<GenericCommand<DeleteInput<Guid, PartnerUnit>, bool>, bool>(new GenericCommand<DeleteInput<Guid, PartnerUnit>, bool>(new DeleteInput<Guid, PartnerUnit>(id))));
        }
    }
}
