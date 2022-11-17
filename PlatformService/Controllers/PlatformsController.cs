using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ILogger<PlatformsController> _logger;
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;

    public PlatformsController(
        ILogger<PlatformsController> logger,
        IPlatformRepo repository, IMapper mapper, ICommandDataClient commandDataClient)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        var platforms = _repository.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    
    [HttpGet("{id:int}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        var platform = _repository.GetPlatformById(id);
        if (platform is not null)
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        
        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto createPlatform)
    {
        var platform = _mapper.Map<Platform>(createPlatform);
        _repository.CreatePlatform(platform);

        if (_repository.SaveChanges())
        {   //NOTE the platform is not updated (with ID) until save changes is called
            var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
            try
            {
                //Testing communications between services
                await _commandDataClient.SendPlatformToCommandAsync(platformReadDto);
            }
            catch (Exception ex)
            {
                // ignored
                _logger.LogError(ex, "Unable to post to Command Service");
                
            }
            return CreatedAtRoute(nameof(GetPlatformById), new {id=platform.Id}, platformReadDto);
        }
        
        return Problem();
    }

}