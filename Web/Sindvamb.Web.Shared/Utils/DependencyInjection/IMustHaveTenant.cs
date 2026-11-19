namespace Sindvamb.Web.Shared.Utils.DependencyInjection;

public interface IMustHaveTenant
{
    public int TenantId { get; set; }
}
