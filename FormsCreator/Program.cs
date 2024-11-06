using FormsCreator.Application;
using FormsCreator.Application.Filters;
using FormsCreator.Application.Utils;
using FormsCreator.Infrastructure;
using FormsCreator.Infrastructure.Utils;

namespace FormsCreator
{
    internal static class Program
    {
        static WebApplication ConfigureBuilder(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllersWithViews(x =>
            {
                x.Filters.Add<InputValidationFilter>();
            });
            builder.Services.AddPostgreDbContext(builder.Configuration.GetConnectionString("DbConnection"));
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();
            builder.Services.AddImageManager(
                opts => opts.SetCloudName(builder.Configuration["Cloudinary:CloudName"])
                        .SetApiKey(builder.Configuration["Cloudinary:ApiKey"])
                        .SetApiSecret(builder.Configuration["Cloudinary:ApiSecret"])
                        .SetImagePath(builder.Configuration["Cloudinary:ImagePath"]));
            builder.Services.AddJwtBearerAuth(o
                => o.SetSecretKey(builder.Configuration["JWTConfig:SecretKey"])
                    .SetIssuer(builder.Configuration["JWTConfig:Issuer"])
                    .SetAudience(builder.Configuration["JWTConfig:Audience"])
                    .SetExpiresInHours(builder.Configuration.GetValue<double>("JWTConfig:ExpiresInHours")),
                    builder.Environment);
            builder.Services.AddGoogleOAuth(opts =>
            {
                opts.ClientId = builder.Configuration["OAuth:Google:ClientId"]!;
                opts.ClientSecret = builder.Configuration["OAuth:Google:ClientSecret"]!;
            });
            builder.Services.AddSalesforceManager(opts
                => opts.SetClientSecret(builder.Configuration["Salesforce:ClientSecret"])
                .SetClientId(builder.Configuration["Salesforce:ClientId"])
                .SetUserName(builder.Configuration["Salesforce:Username"])
                .SetPassword(builder.Configuration["Salesforce:Password"])
                .SetLoginUrl(builder.Configuration["Salesforce:LoginUrl"])
                .SetRestAccountUrl(builder.Configuration["Salesforce:RestAccountUrl"])
                .SetRestContactUrl(builder.Configuration["Salesforce:RestContactUrl"])
                .SetRestQueryUrl(builder.Configuration["Salesforce:RestQueryUrl"]));
            builder.Services.AddJiraManager(opts =>
            {
                opts.ProjectName = builder.Configuration["Atlassian:ProjectName"]!;
                opts.EmailAccount = builder.Configuration["Atlassian:EmailAccount"]!;
                opts.ApiUrl = builder.Configuration["Atlassian:ApiUrl"]!;
                opts.ApiToken = builder.Configuration["Atlassian:ApiToken"]!;
            });
            builder.Services.AddEmailSender(o => { });

            return builder.Build();
        }

        static void Main(string[] args)
        {
            var app = ConfigureBuilder(WebApplication.CreateBuilder(args));

            if (!app.EnsureDatabaseCreated()) return;

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseChangeLanguage();
            app.UseForwardedHeaders();
            app.UseAuthRedirection();
            app.UseJwtBearerAuth();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
