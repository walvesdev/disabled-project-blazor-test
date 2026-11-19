#region

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Filters.CustomExceptions;
using Sindvamb.Utils.Helpers;

#endregion

namespace Sindvamb.Utils.Extension;

public static class ServiceCollectionExtensions
{
    public static bool IsAdded<T>(this IServiceCollection services) => services.IsAdded(typeof(T));

    public static bool IsAdded(this IServiceCollection services, Type type) => services.Any(d => d.ServiceType == type);

    public static T? GetSingletonInstanceOrNull<T>(this IServiceCollection services) => (T)services.FirstOrDefault(d => d.ServiceType == typeof(T))?.ImplementationInstance;

    public static T GetSingletonInstance<T>(this IServiceCollection services) =>
        services.GetSingletonInstanceOrNull<T>() ??
        throw new CustomException("Não foi possível encontrar o serviço singleton: " +
                                  typeof(T).AssemblyQualifiedName);


    public static IServiceCollection AddDependencyInjectionByConvention(this IServiceCollection services,
        Assembly assemblie)
    {
        DependencyInjectionHelper.AddByConvention(services, assemblie);

        return services;
    }
}
