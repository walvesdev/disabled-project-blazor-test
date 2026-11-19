using Sindvamb.Web.Shared.Models;
using Sindvamb.Web.Shared;

namespace Sindvamb.Web.Shared.Models;

/// <summary>
/// Grid filters translation provider (delegate).
/// </summary>
public delegate Task<IEnumerable<FilterOperatorInfo>> GridFiltersTranslationDelegate();
