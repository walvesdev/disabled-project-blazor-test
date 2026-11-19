using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class CustomerTypeConfig : IEntityTypeConfiguration<CustomerType>
{
    public void Configure(EntityTypeBuilder<CustomerType> builder)
    {
        builder.HasIndex(p => p.Code).IsUnique(true);
    }
}
