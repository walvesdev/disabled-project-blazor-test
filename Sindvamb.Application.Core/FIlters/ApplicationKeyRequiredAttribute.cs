#region

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sindvamb.Application.Core.FIlters;

#endregion

namespace Sindvamb.Application.Core.FIlters
{
    public class ApplicationKeyRequiredAttribute : TypeFilterAttribute
    {
        public ApplicationKeyRequiredAttribute() : base(typeof(ValidateApplicationKeyRequiredFilter))
        {
        }

        private class ValidateApplicationKeyRequiredFilter : IAsyncActionFilter
        {
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                try
                {
                    var decrypted = context.HttpContext.Request.Headers["Authorization"];
                }
                catch
                {
                    //context.Result = new UnauthorizedObjectResult("Erro! A chave Integration-Key informada é inválida.");
                    return;
                }


                await next();
            }
        }
    }
}
