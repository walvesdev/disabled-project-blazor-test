using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class OrderFileControlType : IEntityTypeConfiguration<OrderFileControl>
{
    public void Configure(EntityTypeBuilder<OrderFileControl> builder)
    {

    }
}
