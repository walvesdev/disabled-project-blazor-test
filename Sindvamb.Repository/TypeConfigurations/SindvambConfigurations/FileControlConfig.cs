using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class FileControlConfig : IEntityTypeConfiguration<FileControl>
{
    public void Configure(EntityTypeBuilder<FileControl> builder)
    {

        builder.HasKey(p => p.Id);

        builder.HasOne(e => e.Layout)
            .WithMany(f => f.FileControl)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
