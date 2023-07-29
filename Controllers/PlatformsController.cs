using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    
    public PlatformsController(IPlatformRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
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
    public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        // Our repository works with models and not with DTOs so we need to map from PlatformCreateDto to Platform Model
        var platformModel = _mapper.Map<Platform>(platformCreateDto);
        
        _repository.CreatePlatform(platformModel);
        _repository.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
        
        return CreatedAtRoute("GetPlatformById", new { Id = platformReadDto.Id }, platformReadDto);
    }


}