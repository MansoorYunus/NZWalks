using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper Mapper, IWalkRepository walkRepository)
        {
            mapper = Mapper;
            _walkRepository = walkRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AddWalkRequestDTO request)
        {
            var walkDomainModel = mapper.Map<Walk>(request);

            return Ok(mapper.Map<AddWalkRequestDTO>(await _walkRepository.createAsync(walkDomainModel)));
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortOn, [FromQuery] bool isAsc, 
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            return Ok(mapper.Map<List<AddWalkRequestDTO>>(await _walkRepository.GetAsync(filterOn, filterQuery, sortOn, isAsc, pageNumber, pageSize)));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetId([FromRoute] Guid id)
        {
            return Ok(mapper.Map<AddWalkRequestDTO>(await _walkRepository.GetIdAsync(id)));
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO request)
        {
            return Ok(mapper.Map<AddWalkRequestDTO>(await _walkRepository.UpdateAsync(id, request)));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok(mapper.Map<AddWalkRequestDTO>(await _walkRepository.DeleteAsync(id)));
        }
    }
}
