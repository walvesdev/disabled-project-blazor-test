using Sindvamb.Web.Shared;
namespace Sindvamb.Web.Shared.Models;

public class BreadcrumbItem
{
    #region Properties, Indexers

    public string? Href { get; set; }
    public bool IsCurrentPage { get; set; }
    public string? Text { get; set; }

    #endregion
}
