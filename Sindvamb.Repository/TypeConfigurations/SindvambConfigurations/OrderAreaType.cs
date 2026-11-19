using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class ProductAreaType : IEntityTypeConfiguration<ProductArea>
{
    public void Configure(EntityTypeBuilder<ProductArea> builder)
    {
        builder.HasIndex(p => p.Code).IsUnique(true);
    }
}
