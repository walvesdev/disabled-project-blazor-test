#region

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sindvamb.Web.Shared.Extensions;
using Sindvamb.Web.Shared.Helpers;
using Sindvamb.Web.Shared.Utils.Attributes;
using Sindvamb.Web.Shared.Utils.DependencyInjection;

#endregion

namespace Sindvamb.Web.Shared.Helpers;

public static class DependencyInjectionHelper
{
    private static List<Assembly> GetAllReferencedAssemblies()
    {
        var rootAssembly = Assembly.GetEntryAssembly();
        //var solutionName = rootAssembly.FullName.Split(".").FirstOrDefault();

        var visited = new HashSet<string>();
        var queue = new Queue<Assembly>();

        queue.Enqueue(rootAssembly);

        var asm = new List<Assembly>();

        while (queue.Any())
        {
            var assembly = queue.Dequeue();
            visited.Add(assembly.FullName);

            var references = assembly.GetReferencedAssemblies();//.Where(a => a.FullName.Contains(solutionName));

            foreach (var reference in references)
            {
                if (reference.FullName.Contains("Sindvamb"))
                {
                    if (!visited.Contains(reference.FullName))
                    {
                        visited.Add(reference.FullName);
                        var loaded = Assembly.Load(reference);
                        queue.Enqueue(loaded);
                        asm.Add(loaded);
                    }
                }
            }

        }

        return asm;
    }

    public static void AddByConvention(IServiceCollection services, Assembly assemblie)
    {
        // var assemblies = AssemblyHelper
        //     .LoadSolutionAssemblies(AppContext.BaseDirectory, SearchOption.AllDirectories);

        //var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("Sindvamb"));
        var assemblies = GetAllReferencedAssemblies();
        //assemblies.Add(assemblie);
        //assemblies.Add(AssemblyHelper.LoadAssemblies("Sindvamb.Security.Application"));

        assemblies.ForEach(assembly =>
        {
            var types = AssemblyHelper.GetAllTypes(assembly)
                .Where(type => type != null && type.IsClass && !type.IsAbstract && !type.IsGenericType).ToList();

            types.ForEach(type =>
            {
                if (type == null) return;

                // if (type.FullName.Contains("Sindvamb.Service"))
                // {
                //     var e = type;
                // }

                var attribute = type.GetCustomAttribute<DependencyInjectionAttribute>(true);

                if (attribute.IsNotNull())
                {
                    if (attribute != null) AddIfHasAttribute(services, type, attribute);
                }
                else
                {
                    AddIfHasNoAttribute(services, type);
                }
            });
        });
    }

    public static void AddIfHasAttribute(IServiceCollection services, Type typeImplement, DependencyInjectionAttribute attribute)
    {
        var exposeService = typeImplement.GetCustomAttribute<ExposeServicesAttribute>(true);

        if (attribute.IsNotNull() && exposeService.IsNotNull())
        {
            if (attribute.ImplementType.IsNotNull() && attribute.ServiceType.IsNull())
            {
                if (typeImplement.IsAssignableTo(attribute.ImplementType)) AddType(services, typeImplement, attribute.Lifetime, attribute.ImplementType);
            }
            else
            {
                var exposeServices = exposeService.GetExposedServiceTypes(typeImplement);

                if (!exposeServices.IsNullOrEmpty())
                    exposeServices.ForEach(exServivice =>
                    {
                        if (typeImplement.IsAssignableTo(exServivice)) AddType(services, typeImplement, attribute.Lifetime, exServivice);
                    });
            }
        }
        else if (attribute.IsNotNull() && exposeService.IsNull() && attribute.ImplementType.IsNotNull())
        {
            var typeIterface = GetTypeIterface(typeImplement);

            if (typeIterface.IsNotNull())
                AddType(services, typeImplement, attribute.Lifetime, typeIterface);
            else if (attribute.ServiceType.IsNotNull())
                AddType(services, attribute.ImplementType, attribute.Lifetime, attribute.ServiceType);
            else
                AddType(services, typeImplement, attribute.Lifetime);
        }
        else if (attribute.IsNotNull() && exposeService.IsNull() && attribute.ImplementType.IsNull() && !attribute.ImplementTypeName.IsNullOrWhiteSpace())
        {
            attribute.ImplementType = GetTypeByName(attribute.ImplementTypeName);

            if (attribute.ServiceType.IsNotNull() && attribute.ImplementType.IsNotNull()) AddType(services, attribute.ImplementType, attribute.Lifetime, attribute.ServiceType);
        }
    }

    public static void AddIfHasNoAttribute(IServiceCollection services, Type type)
    {
        if (type.IsAssignableTo(typeof(ITransientDependency))) AddDependency(services, type, ServiceLifetime.Transient);

        if (type.IsAssignableTo(typeof(IScopedDependency))) AddDependency(services, type, ServiceLifetime.Scoped);

        if (type.IsAssignableTo(typeof(ISingletonDependency))) AddDependency(services, type, ServiceLifetime.Singleton);
    }

    public static void AddDependency(IServiceCollection services, Type typeImplement, ServiceLifetime serviceLifetime) => AddType(services, typeImplement, serviceLifetime, GetTypeIterface(typeImplement));

    public static Type GetTypeIterface(Type type) => type.GetInterfaces().FirstOrDefault(i => i.Name.Equals($"I{type.Name}"));

    private static Type GetTypeByName(string name)
    {
        Type typeResult = null;

        var assemblies = AssemblyHelper
            .LoadSolutionAssemblies(AppContext.BaseDirectory, SearchOption.AllDirectories);

        assemblies.ForEach(assembly =>
        {
            var types = AssemblyHelper.GetAllTypes(assembly)
                .Where(type => type != null && type.IsClass && !type.IsAbstract && !type.IsGenericType).ToList();

            types.ForEach(type =>
            {
                if (type.IsNull()) return;

                if (type.Name.Equals(name)) typeResult = type;
            });
        });

        return typeResult;
    }

    public static void AddType(IServiceCollection services, Type typeImplement, ServiceLifetime serviceLifetime, Type typeIterface = null)
    {
        if (typeIterface.IsNotNull() && typeIterface.IsAssignableFrom(typeImplement))
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(typeIterface, typeImplement);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(typeIterface, typeImplement);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient(typeIterface, typeImplement);
                    break;
            }
        else
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(typeImplement);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(typeImplement);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient(typeImplement);
                    break;
            }
    }
}
