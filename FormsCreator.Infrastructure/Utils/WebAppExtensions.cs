using FormsCreator.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FormsCreator.Infrastructure.Utils
{
    public static class WebAppExtensions
    {
        public static bool EnsureDatabaseCreated(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<FormsDbContext>();

                dbContext.Database.Migrate();
                return true;
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<FormsDbContext>>();
                logger.LogError(ex, "Migrations couldn't be applied. The application will stop.");
                return false;
            }
        }
    }
}
