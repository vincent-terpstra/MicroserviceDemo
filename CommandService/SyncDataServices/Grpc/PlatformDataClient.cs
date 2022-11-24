using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.SyncDataServices.Grpc;

public class PlatformDataClient : IPlatformDataClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public PlatformDataClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }
    
    public IEnumerable<Platform>? ReturnAllPlatforms()
    {
        Console.WriteLine($"calling Grpc Service {_configuration["GrpcPlatform"]}");
        var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);

        var request = new GetAllRequests();

        try
        {
            var reply = client.GetAllPlatforms(request);
            return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
        }
        catch (Exception ex) {
            Console.WriteLine($"--> Could not call GRPC Server {ex.Message}");
            return null;
        }
    }
}