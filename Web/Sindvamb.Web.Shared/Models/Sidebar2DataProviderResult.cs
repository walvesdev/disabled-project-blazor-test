using Sindvamb.Web.Shared.Models;
using Sindvamb.Web.Shared;

namespace Sindvamb.Web.Shared.Models;

public class Sidebar2DataProviderResult
{
    #region Properties, Indexers

    /// <summary>
    /// The provided items by the request.
    /// </summary>
    public IEnumerable<NavItem>? Data { get; init; }

    #endregion
}
