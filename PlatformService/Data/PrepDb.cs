namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrepPopulation(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!);
    }

    private static void SeedData(AppDbContext context)
    {
        if (context.Platforms.Any() == false)
        {
            //TODO use ILogger
            Console.WriteLine("--> Seeding Data");
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
            //TODO use ILogger
            Console.WriteLine("--> We already have data");   
        }
    }
}