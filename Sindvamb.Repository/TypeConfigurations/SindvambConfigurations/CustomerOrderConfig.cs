using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class CustomerOrderConfig : IEntityTypeConfiguration<CustomerOrder>
{
    public void Configure(EntityTypeBuilder<CustomerOrder> builder)
    {
        builder.HasOne(p => p.Order)
            .WithMany(o => o.CustomerOrder)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.Customer)
            .WithMany(c => c.CustomerOrder)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
