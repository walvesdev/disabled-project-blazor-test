using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class OrderTypeConfig : IEntityTypeConfiguration<OrderType>
{
    public void Configure(EntityTypeBuilder<OrderType> builder)
    {
        builder.HasIndex(p => p.Code).IsUnique(true);
    }
}
