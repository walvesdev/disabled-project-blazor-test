using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class DependentConfig : IEntityTypeConfiguration<Dependent>
{
    public void Configure(EntityTypeBuilder<Dependent> builder)
    {

        builder.HasKey(p => p.Id);

        builder.HasMany(e => e.FileControl)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
