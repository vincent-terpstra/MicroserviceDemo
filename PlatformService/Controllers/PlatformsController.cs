using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;
    
    public PlatformsController(IPlatformRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        var platforms = _repository.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    
    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        var platform = _repository.GetPlatformById(id);
        if (platform is not null)
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        
        return NotFound();
        
        
    }

    [HttpPost]
    public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto createPlatform)
    {
        var platform = _mapper.Map<Platform>(createPlatform);
        _repository.CreatePlatform(platform);

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
        
        if (_repository.SaveChanges())
            return CreatedAtRoute(nameof(GetPlatformById), new {id=platformReadDto.Id}, platformReadDto);

        return Problem();
    }

}