using Sindvamb.Domain.DTO;

namespace Sindvamb.Templates.Pages.Demandas;

public class OrderCreate : BasePage
{
    public string OrderId { get; set; }
    public string OrderProtocol { get; set; }
    public string BusinnesPartnerName { get; set; }
    public string PartnerUnitName { get; set; }
    public string ProductAreaName { get; set; }
    public string ClientName { get; set; }
    public string ClientEmail { get; set; }
    public string OrderTypeName { get; set; }
    public string OrderDescription { get; set; }
    public string ProductCategoryName { get; set; }
    public string FrontEndUrl { get; set; }
}