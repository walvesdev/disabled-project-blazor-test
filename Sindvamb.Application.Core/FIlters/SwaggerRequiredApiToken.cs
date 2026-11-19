#region

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

#endregion

namespace Sindvamb.Application.Core.FIlters
{
    public class SwaggerRequiredApiToken : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();


            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Required = false
            });
        }
    }
}
