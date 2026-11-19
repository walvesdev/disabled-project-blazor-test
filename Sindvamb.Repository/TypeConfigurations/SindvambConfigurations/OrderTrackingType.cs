using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class OrderTrackingType : IEntityTypeConfiguration<OrderTracking>
{
    public void Configure(EntityTypeBuilder<OrderTracking> builder)
    {

    }
}
