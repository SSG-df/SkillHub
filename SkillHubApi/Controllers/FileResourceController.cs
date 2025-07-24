using Microsoft.AspNetCore.Mvc;
using SkillHubApi.Dtos;
using SkillHubApi.Services;

namespace SkillHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileResourceController : ControllerBase
    {
        private readonly IFileResourceService _fileResourceService;

        public FileResourceController(IFileResourceService fileResourceService)
        {
            _fileResourceService = fileResourceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var files = await _fileResourceService.GetAllAsync();
            return Ok(files);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var file = await _fileResourceService.GetByIdAsync(id);
            if (file == null) return NotFound();
            return Ok(file);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FileResourceCreateDto dto)
        {
            await _fileResourceService.AddAsync(dto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FileResourceUpdateDto dto)
        {
            await _fileResourceService.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _fileResourceService.DeleteAsync(id);
            return Ok();
        }
    }
}

