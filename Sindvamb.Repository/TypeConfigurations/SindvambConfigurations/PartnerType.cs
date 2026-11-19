using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class PartnerType : IEntityTypeConfiguration<Partner>
{
    public void Configure(EntityTypeBuilder<Partner> builder)
    {

    }
}
