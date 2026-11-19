namespace Sindvamb.Utils.Services.DependencyInjection;

public interface IMustHaveTenant
{
    public int TenantId { get; set; }
}
