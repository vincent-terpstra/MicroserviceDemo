using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient :ICommandDataClient
{
    private readonly ILogger<HttpCommandDataClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public HttpCommandDataClient(ILogger<HttpCommandDataClient> logger, HttpClient httpClient, IConfiguration config)
    {
        _logger = logger;
        _httpClient = httpClient;
        _config = config;
    }
    
    public async Task SendPlatformToCommandAsync(PlatformReadDto platform)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platform), 
            Encoding.UTF8, 
            "application/json"
            );

        var response = await _httpClient.PostAsync(_config["CommandService"], httpContent);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("-->Sync POST to Command Service was OK!");
        }
        else
        {
            _logger.LogError("-->Sync POST to Command Service was NOT OK!");
        }
    }
}