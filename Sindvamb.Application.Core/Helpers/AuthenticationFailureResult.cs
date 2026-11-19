using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Sindvamb.Application.Core.Helpers;

public class AuthenticationFailureResult : IActionResult
{
    public async Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult("Acesso não autorizado!")
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        };

        await objectResult.ExecuteResultAsync(context);
    }
}

public class AuthenticationFailureResult<T> : IActionResult
{
    public T Item { get; set; }
    public AuthenticationFailureResult(T obj)
    {
        Item = obj;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(Item)
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        };

        await objectResult.ExecuteResultAsync(context);
    }
}
