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
    public class CustomerTypeController : AppControllerBase
    {
        public IMediatorHandler MediatorHandler { get; }

        public CustomerTypeController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            MediatorHandler = mediatorHandler;
        }


        /// <summary>
        /// Retorna uma lista de Tipo de Cliente.
        /// </summary>
        /// <remarks> Retorna uma lista de Tipo de Cliente. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<List<CustomerTypeDTO>>, List<CustomerTypeDTO>>(new GenericCommand<List<CustomerTypeDTO>>()));
        }

        /// <summary>
        /// Retorna um Tipo de Cliente.
        /// </summary>
        /// <remarks> Retorna um Tipo de Cliente. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<Guid, CustomerTypeDTO>, CustomerTypeDTO>(new GenericCommand<Guid, CustomerTypeDTO>(id)));
        }

        /// <summary>
        /// Cadastro de Tipo de Cliente.
        /// </summary>
        /// <remarks> Cadastra um novo Tipo de Cliente. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerTypeDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<CreateInput<CustomerTypeDTO>, bool>, bool>(new GenericCommand<CreateInput<CustomerTypeDTO>, bool>(new CreateInput<CustomerTypeDTO>(item))));
        }

        /// <summary>
        /// Atualização de Tipo de Cliente.
        /// </summary>
        /// <remarks> Atualização de Partne. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CustomerTypeDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<UpdateInput<CustomerTypeDTO>, bool>, bool>(new GenericCommand<UpdateInput<CustomerTypeDTO>, bool>(new UpdateInput<CustomerTypeDTO>(item))));
        }

        /// <summary>
        /// Deleta um Tipo de Cliente.
        /// </summary>
        /// <remarks> Deleta um Tipo de Cliente. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<DeleteInput<Guid, CustomerType>, bool>, bool>(new GenericCommand<DeleteInput<Guid, CustomerType>, bool>(new DeleteInput<Guid, CustomerType>(id))));
        }
    }
}
