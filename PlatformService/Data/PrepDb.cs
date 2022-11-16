namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrepPopulation(this IApplicationBuilder app, Serilog.ILogger logger)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!, logger);
    }

    private static void SeedData(AppDbContext context, Serilog.ILogger logger)
    {
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