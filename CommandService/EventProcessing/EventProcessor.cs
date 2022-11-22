using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.EventProcessing;

/// <summary>
/// Takes message
/// determines what it is
/// process the message/event
/// </summary>
public class EventProcessor :IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    /// Using IServiceScopeFactory because the Repository is scoped while EventProcessor is Singleton (Dependancy Injection)
    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);
        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                break;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using var scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

        try
        {
            var platform = _mapper.Map<Platform>(platformPublishedDto);
            if (!repo.ExternalPlatformExists(platform.ExternalId))
            {
                repo.CreatePlatform(platform);
                repo.SaveChanges();
            }
            else
            {
                Console.WriteLine("-->Platform Exists");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not add platform to db {e.Message}");
            throw;
        }
    }

    private EventType DetermineEvent(string message)
    {
        Console.WriteLine("-->Determining event ");
        try
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);

            return eventType!.Event switch
            {
                "Platform_Published" => EventType.PlatformPublished,
                _ => EventType.Undetermined
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to determine EventType for {message}");
            Console.WriteLine(ex.Message);
            return EventType.Undetermined;
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}