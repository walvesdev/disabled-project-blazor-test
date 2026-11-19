#region

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sindvamb.Application.Core.Attributes;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Handlers;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Inputs;

#endregion

namespace Sindvamb.Application.Core;

[ApiController]
[Route("api/[Controller]/[Action]")]
[ApiSettings]
// [Permission(AuthPolicy.MASTER_ADMIN)]
// [Authorize(Roles = AuthRole.MASTER_ADMIN)]
public abstract class AppControllerCrudBase<TEntity, TEntityDTO, TKey> : AppControllerBase, IDisposable
{
    protected AppControllerCrudBase(
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
    {
        Notifications = (GenericNotificationHandler<DomainNotification>)notifications;
        MediatorHandler = mediatorHandler;
    }
    
     /// <summary>
        /// Retorna uma lista de Registro.
        /// </summary>
        /// <remarks> Retorna uma lista de Registro. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<List<TEntityDTO>>, List<TEntityDTO>>(new GenericCommand<List<TEntityDTO>>()));
        }

        /// <summary>
        /// Retorna um Registro.
        /// </summary>
        /// <remarks> Retorna um Registro. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<Guid, TEntityDTO>, TEntityDTO>(new GenericCommand<Guid, TEntityDTO>(id)));
        }

        /// <summary>
        /// Cadastro de Registro.
        /// </summary>
        /// <remarks> Cadastra um novo Registro. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TEntityDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<CreateInput<TEntityDTO>, bool>, bool>(new GenericCommand<CreateInput<TEntityDTO>, bool>(new CreateInput<TEntityDTO>(item))));
        }

        /// <summary>
        /// Atualização de Registro.
        /// </summary>
        /// <remarks> Atualização de Registro. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TEntityDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<UpdateInput<TEntityDTO>, bool>, bool>(new GenericCommand<UpdateInput<TEntityDTO>, bool>(new UpdateInput<TEntityDTO>(item))));
        }

        /// <summary>
        /// Deleta um Registro.
        /// </summary>
        /// <remarks> Deleta um Registro. </remarks>
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
                .SendCommand<GenericCommand<DeleteInput<Guid, TEntity>, bool>, bool>(new GenericCommand<DeleteInput<Guid, TEntity>, bool>(new DeleteInput<Guid, TEntity>(id))));
        }

}
