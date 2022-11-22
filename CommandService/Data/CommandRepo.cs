using CommandService.Models;

namespace CommandService.Data;

public class CommandRepo :ICommandRepo
{
    private readonly AppDbContext _context;

    public CommandRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public bool SaveChanges()
    {
        return _context.SaveChanges() > 0;
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _context.Platforms;
    }

    public void CreatePlatform(Platform platform)
    {
        _ = platform ?? throw new ArgumentNullException(nameof(platform));

        _context.Platforms.Add(platform);
    }

    public bool PlatformExists(int platformId)
    {
        return _context.Platforms.Any(p => p.Id == platformId);
    }

    public bool ExternalPlatformExists(int externalPlatformId)
    {
        return _context.Platforms.Any(p => p.ExternalId == externalPlatformId);
    }

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
        return _context.Commands.Where(c => c.PlatformId == platformId);
    }

    public Command? GetCommand(int platformId, int commandId)
    {
        return _context.Commands.FirstOrDefault(c => c.PlatformId == platformId && c.Id == commandId);
    }

    public void CreateCommand(int platformId, Command command)
    {
        _ = command ?? throw new ArgumentNullException();

        command.PlatformId = platformId;
        _context.Commands.Add(command);
    }
}