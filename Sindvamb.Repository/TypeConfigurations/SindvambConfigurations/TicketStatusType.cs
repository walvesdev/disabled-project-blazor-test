using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class TicketStatusType : IEntityTypeConfiguration<TicketStatus>
{
    public void Configure(EntityTypeBuilder<TicketStatus> builder)
    {
        builder.HasIndex(p => p.Code).IsUnique(true);
    }
}
