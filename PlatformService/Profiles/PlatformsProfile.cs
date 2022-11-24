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
        CreateMap<Platform, GrpcPlatformModel>().ForMember(
            dest=>dest.PlatformId,
            opt => opt.MapFrom(src => src.Id))
            .ForMember(
                dest=>dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(
                dest=>dest.Publisher,
                opt => opt.MapFrom(src => src.Publisher));
    }
}
