#region

using Microsoft.AspNetCore.Mvc.ModelBinding;

using Newtonsoft.Json;

#endregion

namespace Sindvamb.Application.Core.Helpers;

public class EmptyModelBinder<T> : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var stream = bindingContext.HttpContext.Request.Body;
        using var reader = new StreamReader(stream);
        var jsonbody = await reader.ReadToEndAsync();
        var obj = JsonConvert.DeserializeObject<T>(jsonbody);
        bindingContext.Result = ModelBindingResult.Success(obj);
    }
}
