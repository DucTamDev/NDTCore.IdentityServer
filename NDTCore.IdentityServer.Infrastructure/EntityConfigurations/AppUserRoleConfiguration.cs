using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NDTCore.IdentityServer.Domain.Entities;

namespace NDTCore.IdentityServer.Infrastructure.EntityConfigurations
{
    public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.ToTable("AspNetUserRoles");

            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.HasOne<AppUser>()
                   .WithMany()
                   .HasForeignKey(ur => ur.UserId)
                   .IsRequired();

            builder.HasOne<AppRole>()
                   .WithMany()
                   .HasForeignKey(ur => ur.RoleId)
                   .IsRequired();
        }
    }
}
