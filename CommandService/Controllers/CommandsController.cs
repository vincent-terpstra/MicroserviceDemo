using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("/api/c/platforms/{platformId}/[controller]/")]
public class CommandsController :ControllerBase
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepo commandRepo, IMapper mapper)
    {
        _commandRepo = commandRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<CommandReadDto> GetCommandsForPlatform(int platformId)
    {
        if (_commandRepo.PlatformExists(platformId) == false)
            return NotFound();
        
        var commands = _commandRepo.GetCommandsForPlatform(platformId);
        
        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet]
    [Route("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        if (_commandRepo.PlatformExists(platformId) == false)
            return NotFound();
        
        var command = _commandRepo.GetCommand(platformId, commandId);

        if (command is null)
            return NotFound();

        return Ok(_mapper.Map<CommandReadDto>(command));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
    {
        if (_commandRepo.PlatformExists(platformId) == false)
            return NotFound();

        var command = _mapper.Map<Command>(commandCreateDto);
        _commandRepo.CreateCommand(platformId, command);

        if (_commandRepo.SaveChanges())
        {
            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId, commandId = commandReadDto.Id}, commandReadDto);
        }

        return Problem("Unable to add command");
    }
    
}