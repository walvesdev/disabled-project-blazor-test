using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class RoleType : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {

        builder.HasKey(p => p.Id);

    }
}
