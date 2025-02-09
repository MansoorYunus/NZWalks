using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDBContext dbcontext;
        private readonly IRegionInterface RegionInterface;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDBContext dBContext, IRegionInterface regionInterface, IMapper mapper, ILogger<RegionsController> logger)
        {
            
            dbcontext = dBContext;
            RegionInterface = regionInterface;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()

        {
            logger.LogInformation("logging the info");
            var regions = await RegionInterface.GetAllAsync();
            throw new Exception("hahah error");
            var regionDto = mapper.Map<List<RegionDto>>(regions);

            return Ok(regionDto);
        }

        //get region by ID
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var region = await RegionInterface.GetRegionById(id);

            if (region != null)
            {
                var regionDto = mapper.Map<RegionDto>(region);
                return Ok(regionDto);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto request)

        {
            var region = await RegionInterface.Create(mapper.Map<Region>(request));

            var regionDto = mapper.Map<RegionDto>(region);

            return CreatedAtAction(nameof(Get), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] AddRegionRequestDto request)
        {
            var region = await RegionInterface.Update(mapper.Map<Region>(request), id);

            if (region != null)
            {
                var regionDto = mapper.Map<RegionDto>(region);

                return CreatedAtAction(nameof(Get), new { id = regionDto.Id }, regionDto);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await RegionInterface.Delete(id);
            if (region == null)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }
    }
}
