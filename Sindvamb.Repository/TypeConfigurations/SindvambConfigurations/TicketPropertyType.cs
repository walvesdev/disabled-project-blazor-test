using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class TicketPropertyType : IEntityTypeConfiguration<TicketProperty>
{
    public void Configure(EntityTypeBuilder<TicketProperty> builder)
    {

    }
}
