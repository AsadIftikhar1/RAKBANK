using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlatformController:ControllerBase
	{
		private readonly IPlatformRepo _platformRepo;
		private readonly IMapper _mapper;

		public PlatformController(IPlatformRepo platformRepo,IMapper mapper)
        {
            _platformRepo=platformRepo;
            _mapper = mapper;
        }

		[HttpGet]
		public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
		{
			Console.WriteLine("---- Getting Platforms");
			var platformItems=_platformRepo.GetAllPlatforms();
			return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
		}
		[HttpGet("{id}",Name="GetPlatformById")]
		public ActionResult<PlatformReadDto> GetPlatformById(string Id)
		{
			var platformitem=_platformRepo.GetPlatformById(Convert.ToInt16(Id));
			if(platformitem == null)
			{
				return Ok(_mapper.Map<PlatformReadDto>(platformitem));
				
			}
			return NotFound();
		}

		[HttpPost]
		public ActionResult<PlatformReadDto> CreatePlatformDto(PlatformCreateDto platform)
		{
			var platformcreated = _mapper.Map<Platform>(platform);
			_platformRepo.CreatePlatform(platformcreated);
			_platformRepo.SaveChanges();
			var platformReadDto=_mapper.Map<PlatformReadDto>(platformcreated);
			//Used to return the URL to check wheter the item has created with status code 201;
			return CreatedAtRoute(nameof(GetPlatformById), new { Id=platformcreated.ID},platformReadDto);
		}

	}
}
