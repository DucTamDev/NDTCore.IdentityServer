using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NDTCore.IdentityServer.Domain.Entities;
using NDTCore.IdentityServer.Infrastructure.EntityConfigurations;

namespace NDTCore.IdentityServer.Infrastructure.Persistences
{
    public class ApplicationDbContext : IdentityDbContext<
        AppUser,
        AppRole,
        Guid,
        AppUserClaim,
        AppUserRole,
        AppUserLogin,
        AppRoleClaim,
        AppUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply Configurations
            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new AppRoleConfiguration());
            builder.ApplyConfiguration(new AppUserRoleConfiguration());
            builder.ApplyConfiguration(new AppUserClaimConfiguration());
            builder.ApplyConfiguration(new AppUserLoginConfiguration());
            builder.ApplyConfiguration(new AppUserTokenConfiguration());
            builder.ApplyConfiguration(new AppRoleClaimConfiguration());
        }
    }
}
