using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sindvamb.Domain.Entities;

namespace Sindvamb.Repository.TypeConfigurations.SindvambConfigurations;

public class UserTypeConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //builder.HasMany(e => e.SpecialNeeds)
        //    .WithMany(e => e.Users)
        //    .UsingEntity<SpecialNeedsUser>(
        //        l => l.HasOne<User>(e => e.User).WithMany(e => e.SpecialNeedsUsers).HasForeignKey(e => e.UserId),
        //        r => r.HasOne<SpecialNeeds>(e => e.SpecialNeeds).WithMany(e => e.SpecialNeedsUsers).HasForeignKey(e => e.SpecialNeedsId)
        //        );

        builder.HasKey(p => p.Id);

        builder.HasIndex(u => u.CpfCnpj, "ix_user_cpf_cnpj")
            .IsUnique(true);

        builder.HasMany(e => e.SpecialNeeds)
            .WithOne(u => u.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.LoginHistory)
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.PasswordHistory)
            .WithOne(u => u.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Audit)
            .WithOne(u => u.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.FileControl)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.EducationDegree);

        builder.HasOne(e => e.Role)
            .WithMany(e => e.Users)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(e => e.UserStatus)
            .WithMany(e => e.User)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(e => e.Address)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.PartnerUnit)
            .WithMany(e => e.Users)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
