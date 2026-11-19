#nullable disable

using Sindvamb.Domain;
#nullable disable

using Sindvamb.Domain.Entities;
using Sindvamb.Domain.Helpers;

namespace Sindvamb.Domain.Entities
{
    public class Order : EntityBaseAudit<Guid>
    {
        public Order()
        {
            Id = SequentialGuidGenerator.NewSequentialGuid();
            
            CustomerOrder = new HashSet<CustomerOrder>();
            OrderFileControl = new HashSet<OrderFileControl>();
            OrderProperty = new HashSet<OrderProperty>();
            OrderTracking = new HashSet<OrderTracking>();
            OrderEmails = new HashSet<OrderEmail>();
            Tickets = new HashSet<Ticket>();
        }


        public string Description { get; set; }
    
        public string Sigla { get; set; }
        
        public string Protocol { get; set; }
        
        public DateTime? DueDate { get; set; }

        public bool? Enabled { get; set; }
        
        public int? OrderIndex { get; set; }
        
        public virtual Guid OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        

        public virtual Guid PartnerUnitId { get; set; }
        public virtual PartnerUnit PartnerUnit { get; set; }
        
        public virtual Guid ProductAreaId { get; set; }
        public virtual ProductArea ProductArea { get; set; }
        
        public virtual Guid OrderTypeId { get; set; }
        public virtual OrderType OrderType { get; set; }
        
        public virtual Guid ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        
        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }
        public virtual ICollection<OrderFileControl> OrderFileControl { get; set; }
        public virtual ICollection<OrderEmail> OrderEmails { get; set; }
        public virtual ICollection<OrderProperty> OrderProperty { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<OrderTracking> OrderTracking { get; set; }
    }
}
