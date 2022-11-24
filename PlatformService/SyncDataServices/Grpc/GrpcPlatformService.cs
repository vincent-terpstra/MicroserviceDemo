using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepo _repo;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequests request, ServerCallContext context)
    {
        var allPlatforms = _repo.GetAllPlatforms();
        PlatformResponse response = new();

        foreach (var platform in allPlatforms)
        {
            response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));
        }

        return Task.FromResult(response);

    }
}