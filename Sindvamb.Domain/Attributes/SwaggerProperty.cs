using System.ComponentModel;

namespace Sindvamb.Domain.Attributes;

public class SwaggerPropertyAttribute : DisplayNameAttribute
{
    public string Name { get; set; }
    public string Description { get; set; }

}

public class SwaggerSchemaAttribute : DisplayNameAttribute
{
    public string Name { get; set; }
    public string Description { get; set; }


}