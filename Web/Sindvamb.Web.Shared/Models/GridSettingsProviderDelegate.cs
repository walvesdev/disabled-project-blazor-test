using Sindvamb.Web.Shared.Models;
using Sindvamb.Web.Shared;

namespace Sindvamb.Web.Shared.Models;

/// <summary>
/// Grid settings provider (delegate).
/// </summary>
public delegate Task<GridSettings> GridSettingsProviderDelegate();
