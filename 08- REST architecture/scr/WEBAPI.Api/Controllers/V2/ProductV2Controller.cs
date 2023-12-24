using WEBAPI.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;
using WEBAPI.Domain.Entities;
using WEBAPI.Infrastructure.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WEBAPI.Infrastructure.Query.Request;
using WEBAPI.Common.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;

namespace WEBAPI.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    [ApiController]
    public class ProductV2Controller : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductV2Controller(IMapper mapper, ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        [HttpGet("api/products/count")]
        public async Task<IActionResult> GetCategoriesCount()
        {
            return Ok(await _productRepository.GetCountAsync());
        }

        [HttpGet("api/products")]
        public async Task<IActionResult> GetPagedCategories([FromQuery] GetProductsRequestVm vm)
        {
            var dto = _mapper.Map<GetProductsQueryDto>(vm);
            var product = await _productRepository.GetPagedProductsAsync(dto);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("api/product/{id:int:min(1)}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost("api/product")]
        public async Task<IActionResult> AddCategory([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productRepository.AddAsync(product);
            await _productRepository.SaveEntitiesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = product }, product);
        }

        [HttpPut("api/product")]
        public async Task<IActionResult> EditCategory([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _productRepository.Update(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _productRepository.GetByIdAsync(product.Id) == null)
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("api/product/{id:int:min(1)}")]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            await _productRepository.DeleteByIdAsync(id);
            await _productRepository.SaveEntitiesAsync();

            return NoContent();
        }

        [HttpPatch("api/product/{id:int:min(1)}")]
        public async Task<IActionResult> PartialUpdate(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(product, (IObjectAdapter)ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _productRepository.Update(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _productRepository.GetByIdAsync(product.Id) == null)
                    return NotFound();

                throw;
            }

            return NoContent();
        }
    }
}

