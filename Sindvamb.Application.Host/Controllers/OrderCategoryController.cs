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
    public class ProductCategoryController : AppControllerBase
    {
        public IMediatorHandler MediatorHandler { get; }

        public ProductCategoryController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            MediatorHandler = mediatorHandler;
        }


        /// <summary>
        /// Retorna uma lista de Categoria da Demanda.
        /// </summary>
        /// <remarks> Retorna uma lista de Categoria da Demanda. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<List<ProductCategoryDTO>>, List<ProductCategoryDTO>>(new GenericCommand<List<ProductCategoryDTO>>()));
        }

        /// <summary>
        /// Retorna um Categoria da Demanda.
        /// </summary>
        /// <remarks> Retorna um Categoria da Demanda. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<Guid, ProductCategoryDTO>, ProductCategoryDTO>(new GenericCommand<Guid, ProductCategoryDTO>(id)));
        }

        /// <summary>
        /// Cadastro de Categoria da Demanda.
        /// </summary>
        /// <remarks> Cadastra um novo Categoria da Demanda. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCategoryDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<CreateInput<ProductCategoryDTO>, bool>, bool>(new GenericCommand<CreateInput<ProductCategoryDTO>, bool>(new CreateInput<ProductCategoryDTO>(item))));
        }

        /// <summary>
        /// Atualização de Categoria da Demanda.
        /// </summary>
        /// <remarks> Atualização de Partne. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro Guiderno do servidor. Contate a administração do sistema. </response>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductCategoryDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<UpdateInput<ProductCategoryDTO>, bool>, bool>(new GenericCommand<UpdateInput<ProductCategoryDTO>, bool>(new UpdateInput<ProductCategoryDTO>(item))));
        }

        /// <summary>
        /// Deleta um Categoria da Demanda.
        /// </summary>
        /// <remarks> Deleta um Categoria da Demanda. </remarks>
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
                .SendCommand<GenericCommand<DeleteInput<Guid, ProductCategory>, bool>, bool>(new GenericCommand<DeleteInput<Guid, ProductCategory>, bool>(new DeleteInput<Guid, ProductCategory>(id))));
        }
    }
}
