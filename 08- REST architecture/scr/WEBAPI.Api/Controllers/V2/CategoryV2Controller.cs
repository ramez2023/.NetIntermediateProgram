using AutoMapper;
using WEBAPI.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;
using WEBAPI.Infrastructure.Repositories.Interfaces;
using WEBAPI.Domain.Entities;
using WEBAPI.Infrastructure.Query.Request;
using Microsoft.EntityFrameworkCore;

namespace WEBAPI.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    [ApiController]
    public class CategoryV2Controller : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryV2Controller(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("api/categories/count")]
        public async Task<IActionResult> GetCategoriesCount()
        {
            return Ok(await _categoryRepository.GetCountAsync());
        }

        [HttpGet("api/categories")]
        public async Task<IActionResult> GetPagedCategories([FromQuery] GetCategoriesRequestVm vm)
        {
            var dto = _mapper.Map<GetCategoriesQueryDto>(vm);
            var category = await _categoryRepository.GetPagedCategoriesAsync(dto);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpGet("api/category/{id:int:min(1)}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost("api/category")]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveEntitiesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category }, category);
        }

        [HttpPut("api/category")]
        public async Task<IActionResult> EditCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                _categoryRepository.Update(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _categoryRepository.GetByIdAsync(category.Id) == null)
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("api/category/{id:int:min(1)}")]
        public async Task<IActionResult> DeleteCategoryById([FromRoute] int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            await _categoryRepository.DeleteByIdAsync(id);
            await _categoryRepository.SaveEntitiesAsync();

            return NoContent();
        }

    }
}
