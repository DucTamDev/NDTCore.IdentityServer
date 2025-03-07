using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NDTCore.IdentityServer.Domain.Entities;

namespace NDTCore.IdentityServer.Infrastructure.EntityConfigurations
{
    public class AppUserLoginConfiguration : IEntityTypeConfiguration<AppUserLogin>
    {
        public void Configure(EntityTypeBuilder<AppUserLogin> builder)
        {
            builder.ToTable("AspNetUserLogins");

            builder.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });

            builder.HasOne<AppUser>()
                   .WithMany()
                   .HasForeignKey(ul => ul.UserId)
                   .IsRequired();
        }
    }
}