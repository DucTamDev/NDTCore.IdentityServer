using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NDTCore.IdentityServer.Domain.Entities;

namespace NDTCore.IdentityServer.Infrastructure.EntityConfigurations
{
    public class AppUserClaimConfiguration : IEntityTypeConfiguration<AppUserClaim>
    {
        public void Configure(EntityTypeBuilder<AppUserClaim> builder)
        {
            builder.ToTable("AspNetUserClaims");

            builder.HasKey(uc => uc.Id);

            builder.HasOne<AppUser>()
                   .WithMany()
                   .HasForeignKey(uc => uc.UserId)
                   .IsRequired();
        }
    }
}