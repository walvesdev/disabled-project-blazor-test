#region

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Sindvamb.Application.Core.Attributes;

#endregion

namespace Sindvamb.Application.Core.Conventions;

public class ControllerDocumentationConvention : IControllerModelConvention
{
    void IControllerModelConvention.Apply(ControllerModel controller)
    {
        if (controller == null)
            return;

        var controllerNamespace = controller.ControllerType.Namespace;
        var apiVersion = controllerNamespace.Split(".").Last().ToLower();

        foreach (var attribute in controller.Attributes)
        {
            if (attribute.GetType() == typeof(ApiSettingsAttribute))
            {
                var routeAttribute = (ApiSettingsAttribute)attribute;
                if (!string.IsNullOrWhiteSpace(routeAttribute.Name))
                {
                    controller.ControllerName = routeAttribute.Name;
                }

                if (!string.IsNullOrWhiteSpace(routeAttribute.Version))
                {
                    apiVersion = $"v{routeAttribute.Version}";
                }


                if (routeAttribute.Hidden || routeAttribute.HiddenProduction)
                {
                    controller.ApiExplorer.IsVisible = true;
                }
            }
        }

        if (!apiVersion.StartsWith("v"))
        {
            apiVersion = "v1.0";
        }

        controller.ApiExplorer.GroupName = apiVersion;
    }
}
