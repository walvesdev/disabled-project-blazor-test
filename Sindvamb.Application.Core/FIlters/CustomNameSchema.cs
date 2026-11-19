using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.OpenApi.Models;
using Sindvamb.Domain.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;
using Sindvamb.Utils.Extension;

namespace Sindvamb.Application.Core.FIlters;

public class CustomNameSchema : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties == null)
        {
            return;
        }

        var swaggerSchema = context.Type.GetCustomAttribute<SwaggerSchemaAttribute>();
        if (swaggerSchema.IsNotNull())
        {
            schema.Description = swaggerSchema.Description;
        }

        if (schema.Properties.Any())
        {
            foreach (var propType in context.Type.GetProperties().Where(x => x.CustomAttributes != null && x.CustomAttributes.Any()))
            {
                var swaggerProperty = propType.GetCustomAttribute<SwaggerPropertyAttribute>();

                var schemaProp = schema.Properties.FirstOrDefault(x => x.Key.Equals(propType.Name, StringComparison.InvariantCultureIgnoreCase));

                #region  ENUM DESCRPITION

                // ENUM DESCRPITION

                // string newName = propType.GetCustomAttribute<DataMemberAttribute>()?.Name;
                // if (string.IsNullOrEmpty(newName))
                //     continue;

                // if (schemaProp.Value.Enum != null && schemaProp.Value.Enum.Any())
                // {
                //     for (int i = 0; i < schemaProp.Value.Enum.Count; i++)
                //     {
                //         OpenApiString curr = schemaProp.Value.Enum[i] as OpenApiString;
                //         var memberInfo = propType.PropertyType.GetMember(curr.Value).FirstOrDefault();
                //         string newValue = memberInfo.GetCustomAttribute<EnumMemberAttribute>()?.Value;
                //         if (string.IsNullOrWhiteSpace(newValue))
                //             continue;
                //         OpenApiString newStr = new OpenApiString(newValue);
                //         schemaProp.Value.Enum.Remove(curr);
                //         schemaProp.Value.Enum.Insert(0, newStr);
                //     }
                // }

                #endregion

                if (swaggerProperty.IsNotNull() && !swaggerProperty.Description.IsNullOrWhiteSpace())
                {
                    schemaProp.Value.Description = swaggerProperty.Description;
                }

                if (swaggerProperty.IsNotNull() && !swaggerProperty.Name.IsNullOrWhiteSpace())
                {
                    schemaProp.Value.Title = swaggerProperty.Name;
                }
                else if (swaggerProperty.IsNotNull() && swaggerProperty.IsNotNull())
                {
                    swaggerProperty.Name = schemaProp.Key;
                }

                if (swaggerProperty.IsNotNull() && schemaProp.IsNotNull())
                {
                    var newSchemaProp = new KeyValuePair<string, OpenApiSchema>(swaggerProperty.Name, schemaProp.Value);
                    schema.Properties.Remove(schemaProp);
                    schema.Properties.Add(newSchemaProp);
                }

            }
        }

        var objAttribute = context.Type.GetCustomAttribute<DataContractAttribute>();
        if (objAttribute != default && objAttribute?.Name?.Length > 0)
        {
            schema.Title = objAttribute.Name;
        }
    }
}
