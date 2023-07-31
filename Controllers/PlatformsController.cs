using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    
    public PlatformsController(IPlatformRepository repository, IMapper mapper, ICommandDataClient commandDataClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        var result = _repository.GetPlatforms();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(result));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        var result = _repository.GetPlatformById(id);
        
        if (result != null) return Ok(_mapper.Map<PlatformReadDto>(result));

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        // Our repository works with models and not with DTOs so we need to map from PlatformCreateDto to Platform Model
        var platformModel = _mapper.Map<Platform>(platformCreateDto);
        
        _repository.CreatePlatform(platformModel);
        _repository.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not send synchronously: {e.Message}");
        }
        
        return CreatedAtRoute("GetPlatformById", new { Id = platformReadDto.Id }, platformReadDto);
    }


}