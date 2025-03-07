using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NDTCore.IdentityServer.Domain.Entities;

namespace NDTCore.IdentityServer.Infrastructure.EntityConfigurations
{
    public class AppRoleClaimConfiguration : IEntityTypeConfiguration<AppRoleClaim>
    {
        public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
        {
            builder.ToTable("AspNetRoleClaims");

            builder.HasKey(rc => rc.Id);

            builder.HasOne<AppRole>()
                   .WithMany()
                   .HasForeignKey(rc => rc.RoleId)
                   .IsRequired();
        }
    }
}
