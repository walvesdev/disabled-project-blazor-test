#region

using Microsoft.Extensions.DependencyInjection;

#endregion

namespace Sindvamb.Utils.Attributes;

public class DependencyInjectionAttribute : Attribute
{
    public DependencyInjectionAttribute()
    {
        Lifetime = ServiceLifetime.Scoped;
    }

    public DependencyInjectionAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }

    public virtual ServiceLifetime Lifetime { get; set; }

    public virtual bool TryRegister { get; set; }

    public virtual bool ReplaceServices { get; set; }

    public virtual Type ServiceType { get; set; }

    public virtual Type ImplementType { get; set; }

    public virtual string ImplementTypeName { get; set; }
}
