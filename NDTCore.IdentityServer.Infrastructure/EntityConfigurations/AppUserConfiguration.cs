using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NDTCore.IdentityServer.Domain.Entities;

namespace NDTCore.IdentityServer.Infrastructure.EntityConfigurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AspNetUsers");

            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}