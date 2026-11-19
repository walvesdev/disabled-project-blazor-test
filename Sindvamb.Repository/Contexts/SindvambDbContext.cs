using Microsoft.EntityFrameworkCore;

using Sindvamb.Domain.Entities;
using Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

namespace Sindvamb.Repository.Contexts
{
    public class SindvambDbContext : BaseContext
    {
        public virtual DbSet<AccessControl> AccessControl { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<ApplicationConfig> ApplicationConfig { get; set; }
        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<ContactRequest> ContactRequest { get; set; }
        public virtual DbSet<FileControl> FileControl { get; set; }
        public virtual DbSet<FileLayout> FileLayout { get; set; }
        public virtual DbSet<PasswordHistory> PasswordHistory { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserStatus> UserStatus { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RegistrationRequest> RegistrationRequest { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerType> CustomerType { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrder { get; set; }
        public virtual DbSet<Dependent> Dependent { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<ProductArea> ProductArea { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<OrderEmail> OrderEmail { get; set; }
        public virtual DbSet<OrderFileControl> OrderFileControl { get; set; }
        public virtual DbSet<OrderProperty> OrderProperty { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<OrderTracking> OrderTracking { get; set; }
        public virtual DbSet<OrderType> OrderType { get; set; }
        public virtual DbSet<Partner> Partner { get; set; }
        public virtual DbSet<PartnerUnit> PartnerUnit { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TicketProperty> TicketProperty { get; set; }
        public virtual DbSet<TicketStatus> TicketStatus { get; set; }
        public virtual DbSet<SpecialNeeds> SpecialNeeds { get; set; }
        public virtual DbSet<EducationDegree> EducationDegree { get; set; }

        public SindvambDbContext(DbContextOptions options) : base(options)
        {
            ConxtexName = nameof(SindvambDbContext);
        }

        protected override void OnModelCreatingBase(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("sindvamb");

            modelBuilder.Entity<Attachment>().Property(c => c.Size)
                .HasPrecision(18, 4);

            modelBuilder.Entity<FileControl>().Property(c => c.FileSize)
                .HasPrecision(18, 4);

            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new CustomerTypeConfig());
            modelBuilder.ApplyConfiguration(new CustomerOrderConfig());
            modelBuilder.ApplyConfiguration(new ProductCategoryType());
            modelBuilder.ApplyConfiguration(new ProductAreaType());
            modelBuilder.ApplyConfiguration(new OrderEmailType());
            modelBuilder.ApplyConfiguration(new OrderFileControlType());
            modelBuilder.ApplyConfiguration(new OrderPropertyType());
            modelBuilder.ApplyConfiguration(new OrderStatusType());
            modelBuilder.ApplyConfiguration(new OrderTrackingType());
            modelBuilder.ApplyConfiguration(new OrderConfig());
            modelBuilder.ApplyConfiguration(new OrderTypeConfig());
            modelBuilder.ApplyConfiguration(new PartnerType());
            modelBuilder.ApplyConfiguration(new PartnerUnitType());
            modelBuilder.ApplyConfiguration(new TicketPropertyType());
            modelBuilder.ApplyConfiguration(new TicketStatusType());
            modelBuilder.ApplyConfiguration(new TicketType());
            modelBuilder.ApplyConfiguration(new RoleType());
            modelBuilder.ApplyConfiguration(new UserTypeConfig());
            modelBuilder.ApplyConfiguration(new RegistrationRequestConfig());
            modelBuilder.ApplyConfiguration(new DependentConfig());
            modelBuilder.ApplyConfiguration(new FileControlConfig());
        }
    }
}
