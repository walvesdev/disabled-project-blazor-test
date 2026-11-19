using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(e => e.CustomerType);

        builder.HasOne(e => e.User)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.CustomerOrder)
            .WithOne(e => e.Customer)
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Dependents)
            .WithOne(e => e.Customer)
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
