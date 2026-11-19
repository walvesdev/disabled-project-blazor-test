using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sindvamb.Application.Core;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Service.Commands.Customers;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Inputs;

namespace Sindvamb.Application.Host.Controllers
{
    public class CustomerController : AppControllerBase
    {
        public IMediatorHandler MediatorHandler { get; }

        public CustomerController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            MediatorHandler = mediatorHandler;
        }


        /// <summary>
        /// Retorna uma lista de Cliente.
        /// </summary>
        /// <remarks> Retorna uma lista de Cliente. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<List<CustomerDTO>>, List<CustomerDTO>>(new GenericCommand<List<CustomerDTO>>()));
        }

        /// <summary>
        /// Retorna um Cliente.
        /// </summary>
        /// <remarks> Retorna um Cliente. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<Guid, CustomerDTO>, CustomerDTO>(new GenericCommand<Guid, CustomerDTO>(id)));
        }

        /// <summary>
        /// Retorna um Cliente.
        /// </summary>
        /// <remarks> Retorna um Cliente com todos os seus dados. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetFullCustomerComand(Guid id)
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GetFullCustomerComand, Customer>(new GetFullCustomerComand(id)));
        }        

        /// <summary>
        /// Cadastro de Cliente.
        /// </summary>
        /// <remarks> Cadastra um novo Cliente. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerDTO item)
        {
           
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<CreateInput<CustomerDTO>, CustomerDTO>, CustomerDTO>(new GenericCommand<CreateInput<CustomerDTO>, CustomerDTO>(new CreateInput<CustomerDTO>(item))));
        }

        /// <summary>
        /// Atualização de Cliente.
        /// </summary>
        /// <remarks> Atualização de Partne. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CustomerDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<UpdateInput<CustomerDTO>, bool>, bool>(new GenericCommand<UpdateInput<CustomerDTO>, bool>(new UpdateInput<CustomerDTO>(item))));
        }

        /// <summary>
        /// Deleta um Cliente.
        /// </summary>
        /// <remarks> Deleta um Cliente. </remarks>
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
                .SendCommand<GenericCommand<DeleteInput<Guid, Customer>, bool>, bool>(new GenericCommand<DeleteInput<Guid, Customer>, bool>(new DeleteInput<Guid, Customer>(id))));
        }

        /// <summary>
        /// Cadastro de Dependente.
        /// </summary>
        /// <remarks> Cadastra um novo Dependente. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpPost]
        public async Task<IActionResult> CreateDependent([FromBody] DependentDTO item)
        {
            return await OkActionResult(await MediatorHandler.SendCommand<CreateDependentComnand, bool>(new CreateDependentComnand(item)));
        }

        /// <summary>
        /// Deleta um dependente.
        /// </summary>
        /// <remarks> Deleta um dependente. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteDependent(Guid id)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<DeleteDependentComnand, bool>(new DeleteDependentComnand(id)));
        }

    }
}