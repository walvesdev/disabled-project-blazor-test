
using Microsoft.Extensions.DependencyInjection;

using Sindvamb.Utils.Extension;

using Sindvamb.Utils.Services.DependencyInjection;
#region


#endregion

namespace Sindvamb.Utils.Services.DependencyInjection;

public class LazyServiceProvider : ILazyServiceProvider, ITransientDependency
{
    public LazyServiceProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        CachedServices = new Dictionary<Type, object>();
    }

    protected IDictionary<Type, object> CachedServices { get; set; }

    protected IServiceProvider ServiceProvider { get; set; }

    public virtual T LazyGetRequiredService<T>() => (T)LazyGetRequiredService(typeof(T));

    public virtual object LazyGetRequiredService(Type serviceType) => CachedServices.GetOrAdd(serviceType, _ => ServiceProvider.GetRequiredService(serviceType));

    public virtual T? LazyGetService<T>() => (T)LazyGetService(typeof(T));

    public virtual object? LazyGetService(Type serviceType) => CachedServices!.GetOrAdd(serviceType, _ => ServiceProvider.GetService(serviceType));

    public virtual T LazyGetService<T>(T defaultValue) => (T)LazyGetService(typeof(T), defaultValue!);

    public virtual object LazyGetService(Type serviceType, object defaultValue) => LazyGetService(serviceType) ?? defaultValue;

    public virtual T LazyGetService<T>(Func<IServiceProvider, object> factory) => (T)LazyGetService(typeof(T), factory);

    public virtual object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory) => CachedServices.GetOrAdd(serviceType, _ => factory(ServiceProvider));
}
