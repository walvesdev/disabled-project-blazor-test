#region

using Sindvamb.Application.Core;
using Sindvamb.Domain.Entities;
using Sindvamb.Security.Services;
using Sindvamb.Service.Commands.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sindvamb.Application.Host.Controllers;
using Sindvamb.Events.BuildingBlocks.Mediator;
using MediatR;
using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;

#endregion

namespace Sindvamb.Application.Host.Controllers
{
    public class AuthController : AppControllerBase
    {
        public LoginService LoginService { get; set; }

        public AuthController(
            LoginService loginService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            LoginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO input)
        {
            var result = await LoginService.Login(new LoginDTO { CpfCnpj = input.CpfCnpj, Password = input.Password }, AuthenticationType.Jwt);
            return await OkActionResult(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Recovery([FromBody] string email)
        {
            await MediatorHandler.SendCommand<RecoveryPasswordCommand, bool>(new RecoveryPasswordCommand(email));
            return await OkActionResult();
        }

        // [AllowAnonymous]
        // [HttpPost("{securityCode}")]
        // public async Task<IActionResult> UpdatePassword([FromBody] string password, [FromRoute] string securityCode)
        // {
        //     if (await MediatorHandler.SendCommand<UpdatePasswordCommand, bool>(new UpdatePasswordCommand(password, securityCode)))
        //     {
        //         return await OkActionResult();
        //     }
        //     
        //     return await UnauthorizedActionResult("Erro ao atualizar senha!");
        // }
    }

}
