using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class OrderEmailType : IEntityTypeConfiguration<OrderEmail>
{
    public void Configure(EntityTypeBuilder<OrderEmail> builder)
    {

    }
}
