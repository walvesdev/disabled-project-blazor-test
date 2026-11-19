using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class OrderPropertyType : IEntityTypeConfiguration<OrderProperty>
{
    public void Configure(EntityTypeBuilder<OrderProperty> builder)
    {

    }
}
