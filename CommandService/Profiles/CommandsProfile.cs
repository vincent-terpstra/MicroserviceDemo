using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        //Source -> Target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();
        //Need to map PlatformPublishedDto.Id to Platform.EventId 
        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(dest => dest.ExternalId, 
                opt => opt.MapFrom(src=> src.Id));
        
        
    }
}