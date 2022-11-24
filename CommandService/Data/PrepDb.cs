using CommandService.Models;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder builder)
    {
        using var serviceScope = builder.ApplicationServices.CreateScope();
        var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>()!;

        var platforms = grpcClient.ReturnAllPlatforms();
        var repo = serviceScope.ServiceProvider.GetService<ICommandRepo>()!;
        
        SeedData(repo, platforms);
    }

    private static void SeedData(ICommandRepo repo, IEnumerable<Platform>? platforms)
    {
        Console.WriteLine("--> Seeding new platforms");

        if (platforms == null)
            return;
        
        foreach (var platform in platforms)
        {
            if(!repo.ExternalPlatformExists(platform.ExternalId))
                repo.CreatePlatform(platform);
        }

        repo.SaveChanges();
    }
}