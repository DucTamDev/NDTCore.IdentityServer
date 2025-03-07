using Microsoft.AspNetCore.Mvc.RazorPages;
using NDTCore.IdentityServer.Application;
using NDTCore.IdentityServer.Infrastructure;
using NDTCore.IdentityServer.Web.Pages.Admin.ApiScopes;
using NDTCore.IdentityServer.Web.Pages.Admin.Clients;
using NDTCore.IdentityServer.Web.Pages.Admin.IdentityScopes;
using Serilog;

namespace NDTCore.IdentityServer.Web;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);

        // this adds the necessary config for the simple admin/config pages
        builder.Services.AddAuthorizationBuilder()
                        .AddPolicy("admin", policy => policy.RequireClaim("sub", "1"));

        builder.Services.Configure<RazorPagesOptions>(options =>
            options.Conventions.AuthorizeFolder("/Admin", "admin"));

        // if you want to use server-side sessions: https://blog.duendesoftware.com/posts/20220406_session_management/
        // then enable it
        //isBuilder.AddServerSideSessions();
        //
        // and put some authorization on the admin/management pages using the same policy created above
        //builder.Services.Configure<RazorPagesOptions>(options =>
        //    options.Conventions.AuthorizeFolder("/ServerSideSessions", "admin"));

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}