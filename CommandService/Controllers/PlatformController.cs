namespace CommandService.Controllers;

public static class PlatformControllerExtensions
{
    public static void MapPlatformController(this WebApplication app)
    {
        app.MapPost("/api/c/platforms", PostPlatforms);
    }
    
    public static IResult PostPlatforms()
    {
        Console.WriteLine("--> Inbound POST # command service");
        return Results.Ok("Test Inbound OK");
    }
}