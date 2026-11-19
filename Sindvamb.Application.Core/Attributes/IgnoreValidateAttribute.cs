using Microsoft.AspNetCore.Mvc.Filters;

namespace Sindvamb.Application.Core.FIlters;

public class IgnoreValidateAttribute : ActionFilterAttribute
{
   
    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        actionContext.ModelState.Clear();     
    }
}