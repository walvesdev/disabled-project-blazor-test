using Sindvamb.Web.Shared;
using Sindvamb.Web.Shared.Models;
namespace Sindvamb.Web.Shared.Models;

/// <summary>
/// Data provider (delegate).
/// </summary>
public delegate Task<AutoCompleteDataProviderResult<TItem>> AutoCompleteDataProviderDelegate<TItem>(AutoCompleteDataProviderRequest<TItem> request);
