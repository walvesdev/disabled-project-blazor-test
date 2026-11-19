using Sindvamb.Web.Shared;
namespace Sindvamb.Web.Shared.Models;

/// <summary>
/// Data provider (delegate).
/// </summary>
public delegate Task<Sidebar2DataProviderResult> Sidebar2DataProviderDelegate(Sidebar2DataProviderRequest request);
