#region

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

#endregion

namespace Sindvamb.Application.Core.Versioning.Filters;

public class RemoveVersionFromParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var versionParameter = operation.Parameters.Single(p => p.Name == "version");
        operation.Parameters.Remove(versionParameter);
    }
}
