using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NDTCore.IdentityServer.Domain.Entities;

namespace NDTCore.IdentityServer.Infrastructure.EntityConfigurations
{
    public class AppUserTokenConfiguration : IEntityTypeConfiguration<AppUserToken>
    {
        public void Configure(EntityTypeBuilder<AppUserToken> builder)
        {
            builder.ToTable("AspNetUserTokens");

            builder.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

            builder.HasOne<AppUser>()
                   .WithMany()
                   .HasForeignKey(ut => ut.UserId)
                   .IsRequired();
        }
    }
}