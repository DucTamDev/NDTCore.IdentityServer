using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NDTCore.IdentityServer.Contract;
using NDTCore.IdentityServer.Domain.Entities;
using NDTCore.IdentityServer.Infrastructure.Persistences;
using NDTCore.IdentityServer.Infrastructure.Repositories;

namespace NDTCore.IdentityServer.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            var migrationsAssembly = typeof(ServiceCollectionExtensions).Assembly.GetName().Name;
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true;
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlite(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlite(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddAspNetIdentity<AppUser>()
                .AddTestUsers(TestUsers.Users);

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddConfigureIdentityServer(configuration);

            services.AddTransient<ClientRepository>();
            services.AddTransient<ClientRepository>();
            services.AddTransient<IdentityScopeRepository>();
            services.AddTransient<ApiScopeRepository>();

            return services;
        }
    }
}
