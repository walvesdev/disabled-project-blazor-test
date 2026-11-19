using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sindvamb.Application.Core;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Commands.Orders;
using Sindvamb.Service.Inputs;

namespace Sindvamb.Application.Host.Controllers
{
    public class OrderController : AppControllerBase
    {
        public IMediatorHandler MediatorHandler { get; }

        public OrderController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            MediatorHandler = mediatorHandler;
        }

         /// <summary>
        /// Retorna uma lista de Demanda.
        /// </summary>
        /// <remarks> Retorna uma lista de Demanda. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<List<OrderDTO>>, List<OrderDTO>>(new GenericCommand<List<OrderDTO>>()));
        }

        /// <summary>
        /// Retorna um Demanda.
        /// </summary>
        /// <remarks> Retorna um Demanda. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<Guid, OrderDTO>, OrderDTO>(new GenericCommand<Guid, OrderDTO>(id)));
        }

        /// <summary>
        /// Cadastro de Demanda.
        /// </summary>
        /// <remarks> Cadastra um novo Demanda. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<CreateInput<OrderDTO>, bool>, bool>(new GenericCommand<CreateInput<OrderDTO>, bool>(new CreateInput<OrderDTO>(item))));
        }

        /// <summary>
        /// Atualização de Demanda.
        /// </summary>
        /// <remarks> Atualização de Partne. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] OrderDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<UpdateInput<OrderDTO>, bool>, bool>(new GenericCommand<UpdateInput<OrderDTO>, bool>(new UpdateInput<OrderDTO>(item))));
        }
        
        /// <summary>
        /// Atualização da situação da Demanda.
        /// </summary>
        /// <remarks> Atualização de Partne. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpPost]
        public async Task<IActionResult> UpdateStatusIndex([FromBody] OrderDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<UpdateStatusIndexCommand, bool>(new UpdateStatusIndexCommand(item)));
        }

        /// <summary>
        /// Deleta um Demanda.
        /// </summary>
        /// <remarks> Deleta um Demanda. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<DeleteInput<Guid, Order>, bool>, bool>(new GenericCommand<DeleteInput<Guid, Order>, bool>(new DeleteInput<Guid, Order>(id))));
        }
    }
}
