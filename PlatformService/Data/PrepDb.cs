using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrepPopulation(this IApplicationBuilder app, Serilog.ILogger logger, bool isProduction)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!, logger, isProduction);
    }

    private static void SeedData(AppDbContext context, Serilog.ILogger logger, bool isProduction)
    {
        if (isProduction)
        {
            try
            {
                logger.Information("Attempting to migrate database");
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Could not run migrations {Message}",ex.Message);
            }
            
        }
        
        
        if (context.Platforms.Any() == false)
        {
            logger.Information("Seeding Data");
            context.Platforms.AddRange(
                new []
                {
                    new Platform(){Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
                    new Platform(){Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free"},
                    new Platform(){Name = "Kubernetes", Publisher = "Cloud Native Computing platform", Cost = "Free"},
                    new Platform(){Name = "Docker", Publisher = "Docker Inc", Cost = "Free"}
                }
            );

            context.SaveChanges();
        }
        else
        {
            logger.Information("We already have data");
        }
    }
}