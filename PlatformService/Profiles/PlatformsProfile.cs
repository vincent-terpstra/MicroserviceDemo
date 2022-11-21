using System.Security.Cryptography.X509Certificates;
using AutoMapper;


namespace PlatformService.Profiles;

public class PlatformsProfile : Profile
{
    public PlatformsProfile()
    {
        //Source -> target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<Platform, PlatformPublishedDto>();
    }
}
