using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class RegistrationRequestConfig : IEntityTypeConfiguration<RegistrationRequest>
{
    public void Configure(EntityTypeBuilder<RegistrationRequest> builder)
    {

        builder.HasKey(p => p.Id);

        builder.HasOne(e => e.User)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
