using Sindvamb.Domain.DTO;

namespace Sindvamb.Domain.DTO;

public class OrderDTO
{
    public OrderDTO()
    {
        CustomerOrder = new HashSet<CustomerOrderDTO>();
        OrderFileControl = new HashSet<OrderFileControlDTO>();
        OrderProperty = new HashSet<OrderPropertyDTO>();
        OrderTracking = new HashSet<OrderTrackingDTO>();
        OrderEmails = new HashSet<OrderEmailDTO>();
        Tickets = new HashSet<TicketDTO>();
    }


    public Guid Id { get; set; }
    public string Description { get; set; }
        
    public string Protocol { get; set; }
    
        
    public DateTime? DueDate { get; set; }

    public bool? Enabled { get; set; }
    
    public DateTime CreatedAt { get; set; }
        
    public virtual Guid OrderStatusId { get; set; }
    public virtual OrderStatusDTO OrderStatus { get; set; }

    public virtual Guid PartnerUnitId { get; set; }
    public virtual PartnerUnitDTO PartnerUnit { get; set; }
        
    public virtual Guid ProductAreaId { get; set; }
    public virtual ProductAreaDTO ProductArea { get; set; }
        
    public virtual Guid OrderTypeId { get; set; }
    public virtual OrderTypeDTO OrderType { get; set; }
        
    public virtual Guid ProductCategoryId { get; set; }
    public virtual ProductCategoryDTO ProductCategory { get; set; }

    public Guid SindvambId { get; set; }
    public Guid PresidencyId { get; set; }
    public Guid ClientId { get; set; }
    
    public Guid DirectorId { get; set; }
    
    public Guid ManagerId { get; set; }
    
    public string Sigla { get; set; }
    
    public bool SendEmailSindvamb { get; set; }
    public bool SendEmailOrderCreator { get; set; }
    public string? EmailWithCopy { get; set; }
        
    public virtual ICollection<CustomerOrderDTO> CustomerOrder { get; set; }
    public virtual ICollection<OrderFileControlDTO> OrderFileControl { get; set; }
    public virtual ICollection<OrderEmailDTO> OrderEmails { get; set; }
    public virtual ICollection<OrderPropertyDTO> OrderProperty { get; set; }
    public virtual ICollection<TicketDTO> Tickets { get; set; }
    public virtual ICollection<OrderTrackingDTO> OrderTracking { get; set; }
    
    public int? OrderIndex { get; set; }
    
}
