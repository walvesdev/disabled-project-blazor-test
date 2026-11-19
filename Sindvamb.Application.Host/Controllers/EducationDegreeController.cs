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
    public class EducationDegreeController : AppControllerBase
    {
        public IMediatorHandler MediatorHandler { get; }

        public EducationDegreeController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            MediatorHandler = mediatorHandler;
        }


        /// <summary>
        /// Retorna uma lista de EducationDegree.
        /// </summary>
        /// <remarks> Retorna uma lista de EducationDegree. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<List<EducationDegreeDTO>>, List<EducationDegreeDTO>>(new GenericCommand<List<EducationDegreeDTO>>()));
        }

        /// <summary>
        /// Retorna um EducationDegree.
        /// </summary>
        /// <remarks> Retorna um EducationDegree. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(int id)
        {
            return await OkActionResult(await MediatorHandler.SendCommand<GenericCommand<int, EducationDegreeDTO>, EducationDegreeDTO>(new GenericCommand<int, EducationDegreeDTO>(id)));
        }

        /// <summary>
        /// Cadastro de EducationDegree.
        /// </summary>
        /// <remarks> Cadastra um novo EducationDegree. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EducationDegreeDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<CreateInput<EducationDegreeDTO>, bool>, bool>(new GenericCommand<CreateInput<EducationDegreeDTO>, bool>(new CreateInput<EducationDegreeDTO>(item))));
        }

        /// <summary>
        /// Atualização de EducationDegree.
        /// </summary>
        /// <remarks> Atualização de Partne. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EducationDegreeDTO item)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<UpdateInput<EducationDegreeDTO>, bool>, bool>(new GenericCommand<UpdateInput<EducationDegreeDTO>, bool>(new UpdateInput<EducationDegreeDTO>(item))));
        }

        /// <summary>
        /// Deleta um EducationDegree.
        /// </summary>
        /// <remarks> Deleta um EducationDegree. </remarks>
        /// <response code="200"> Requisição efetuada com sucesso. </response>
        /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
        /// <response code="400"> Erro ao efetuar requisição. </response>
        /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
        /// <response code="422"> Erro de validação ou na regra de negócio. </response>
        /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await OkActionResult(await MediatorHandler
                .SendCommand<GenericCommand<DeleteInput<int, EducationDegree>, bool>, bool>(new GenericCommand<DeleteInput<int, EducationDegree>, bool>(new DeleteInput<int, EducationDegree>(id))));
        }
    }
}
