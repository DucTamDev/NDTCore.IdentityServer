using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NDTCore.IdentityServer.Contract;
using NDTCore.IdentityServer.Domain.Constants;
using NDTCore.IdentityServer.Domain.Entities;
using NDTCore.IdentityServer.Infrastructure.Persistences;
using NDTCore.IdentityServer.Infrastructure.Repositories;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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

        public static IServiceCollection AddStackOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            var resource = ResourceBuilder
                            .CreateDefault()
                            .AddService(OtelConstants.APP_OTEL_RESOURCE_SERVICE_NAME,
                                        OtelConstants.APP_OTEL_RESOURCE_SERVICE_NAME,
                                        OtelConstants.APP_OTEL_RESOURCE_SERVICE_VERSION);

            services.AddLogging(logging => logging
                    .AddOpenTelemetry(options => options
                        .SetResourceBuilder(resource)
                        .AddConsoleExporter()
                        .AddOtlpExporter()
                    )
            );

            services.AddOpenTelemetry()
                    .WithTracing(tracerBuilder => tracerBuilder
                        .SetResourceBuilder(resource)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation()
                        .AddConsoleExporter()
                        .AddOtlpExporter(options =>
                        {
                            // can get it in appsettings.json
                            options.Endpoint = new Uri(OtelConstants.OTEL_EXPORTER_OTLP_ENDPOINT);
                            options.Protocol = OtlpExportProtocol.HttpProtobuf;
                        })
                    );

            services.AddOpenTelemetry()
                    .WithMetrics(metricBuilder => metricBuilder
                        .SetResourceBuilder(resource)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation()
                        .AddConsoleExporter()
                        .AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri(OtelConstants.OTEL_EXPORTER_OTLP_ENDPOINT);
                            options.Protocol = OtlpExportProtocol.HttpProtobuf;
                        })
                    );

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
