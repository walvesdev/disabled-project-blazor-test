using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sindvamb.Application.Core;
using Sindvamb.Application.Core.FIlters;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Domain.Models;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Commands.Users;
using Sindvamb.Service.Inputs;

namespace Sindvamb.Application.Host.Controllers;

public class UserController : AppControllerBase
{
    public IMediatorHandler MediatorHandler { get; }

    public UserController(
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediatorHandler,
        ISindvambRepository<User, Guid> repository,
        ISindvambRepository<RegistrationRequest, Guid> registrationRequestRepository) : base(notifications,
        mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }

    /// <summary>
    /// Cadastro de usuario.
    /// </summary>
    /// <remarks> Cadastra um novo usuário. </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(UserDTO dto)
    {
        return await OkActionResult(await MediatorHandler.SendCommand<RegisterCommand, bool>(new RegisterCommand(dto)));
    }

    /// <summary>
    /// Atualização de usuario.
    /// </summary>
    /// <remarks> Atualização de usuario. </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpPut]
    public async Task<IActionResult> Update(UserDTO dto)
    {
        return await OkActionResult(await MediatorHandler.SendCommand<UpdateCommand, bool>(new UpdateCommand(dto)));
    }

    /// <summary>
    /// Busca de Usuários.
    /// </summary>
    /// <remarks> Retorna uma lista de usuário. </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return await OkActionResult(
            await MediatorHandler.SendCommand<GetAllCommand, List<UserDTO>>(new GetAllCommand()));
    }

    /// <summary>
    /// Busca as permissoes dos Usuários.
    /// </summary>
    /// <remarks>  Busca as permissoes dos Usuários. </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        return await OkActionResult(
            await MediatorHandler.SendCommand<GenericCommand<List<RoleDTO>>, List<RoleDTO>>(
                new GenericCommand<List<RoleDTO>>()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return await OkActionResult(
            await MediatorHandler.SendCommand<GetUserByIdCommand, UserDTO>(new GetUserByIdCommand(id)));
    }

    /// <summary>
    /// Busca de Usuários pedente de aprovação.
    /// </summary>
    /// <remarks> Retorna uma lista de usuários pedente de aprovação. </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpGet]
    public async Task<IActionResult> GetAllUnauthorizedUsers()
    {
        return await OkActionResult(
            await MediatorHandler.SendCommand<GetAllUnauthorizedUsersCommand, List<RegistrationRequestDTO>>(
                new GetAllUnauthorizedUsersCommand()));
    }

    /// <summary>
    /// Busca status dos Usuários.
    /// </summary>
    /// <remarks> Busca status dos Usuários. </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpGet]
    public async Task<IActionResult> GetAllUserStatus()
    {
        return await OkActionResult(
            await MediatorHandler.SendCommand<GenericCommand<List<UserStatusDTO>>, List<UserStatusDTO>>(
                new GenericCommand<List<UserStatusDTO>>()));
    }

    /// <summary>
    /// Deleta um Usuário.
    /// </summary>
    /// <remarks> Deleta um Usuário. </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await OkActionResult(await MediatorHandler.SendCommand<DeleteCommand, bool>(new DeleteCommand(id)));
    }

    /// <summary>
    /// Aprova um Cadastro de usuario.
    /// </summary>
    /// <remarks> Aprova um Cadastro de usuario.. </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpPost]
    public async Task<IActionResult> ApproveUser(RegistrationRequest dto)
    {
        return await OkActionResult(
            await MediatorHandler.SendCommand<ApproveUserCommand, bool?>(new ApproveUserCommand(dto)));
    }

    /// <summary>
    /// Cadastro de Usuario sem aprovação.
    /// </summary>
    /// <remarks> Cadastro de Usuario sem aprovação. </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserDTO item)
    {
        return await OkActionResult(await MediatorHandler
            .SendCommand<GenericCommand<CreateInput<UserDTO>, bool>, bool>(
                new GenericCommand<CreateInput<UserDTO>, bool>(new CreateInput<UserDTO>(item))));
    }

    [HttpPost]
    public async Task<IActionResult> GetUnauthorizedUserStatus(AcompanharCadastroModel model)
    {
        return await OkActionResult(
            await MediatorHandler.SendCommand<GetUnauthorizedUserStatusCommand, RegistrationRequestDTO>(
                new GetUnauthorizedUserStatusCommand(model)));
    }

    /// <summary>
    /// Reprova um Cadastro de usuario.
    /// </summary>
    /// <remarks> Reprova um Cadastro de usuario.. </remarks>
    /// <response code="200"> Requisição efetuada com sucesso. </response>
    /// <response code="204"> Requisição efetuada com sucesso, Nenhum conteudo será retornado. </response>
    /// <response code="400"> Erro ao efetuar requisição. </response>
    /// <response code="404"> A URL  que você está tentando acessar está indisponível. </response>
    /// <response code="422"> Erro de validação ou na regra de negócio. </response>
    /// <response code="500"> Erro interno do servidor. Contate a administração do sistema. </response>
    [HttpPost]
    public async Task<IActionResult> DenyUser(RegistrationRequest dto)
    {
        return await OkActionResult(
            await MediatorHandler.SendCommand<DenyUserCommand, bool>(new DenyUserCommand(dto)));
    }

    [HttpGet("{cpfCnpj}")]
    public async Task<IActionResult> GetByCpfCnpj(string cpfCnpj)
    {
        return await OkActionResult(
            await MediatorHandler.SendCommand<GetUserByCpfCnpjCommand, Customer>(new GetUserByCpfCnpjCommand(cpfCnpj)));
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePassword(ChangePasswordModel model)
    {
        return await OkActionResult(await MediatorHandler
            .SendCommand<UpdatePasswordCommand, bool>(new UpdatePasswordCommand(model.Password, model.SecurityCode, model.CpfCnpj)));
    }

    [HttpGet("{cpfCnpj}")]
    public async Task<IActionResult> RequestLinkUpdatePasswordCommand(string cpfCnpj)
    {
        return await OkActionResult(
            await MediatorHandler.SendCommand<RequestLinkUpdatePasswordCommand, bool>(new RequestLinkUpdatePasswordCommand(cpfCnpj)));
    }
    
}