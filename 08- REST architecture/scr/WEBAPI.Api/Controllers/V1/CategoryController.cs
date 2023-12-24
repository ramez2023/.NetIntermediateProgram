using WEBAPI.Common.ViewModels;
using WEBAPI.Service.Services.Interfaces;
using WEBAPI.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;
using WEBAPI.Common.Enums;
using WEBAPI.Infrastructure.Query.Response;

namespace WEBAPI.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("api/categories/count")]
        public async Task<Response<int>> GetCategoriesCount([FromQuery] BaseRequestVm requestVm)
        {
            return new Response<int>
            {
                Data = await _categoryService.GetCategoriesCountAsync()
            };
        }

        [HttpGet("api/categories")]
        public async Task<Response<PagedList<GetCategoryResponseVm>>> GetPagedCategories([FromQuery] GetCategoriesRequestVm getCategoriesRequestVm)
        {
            return new Response<PagedList<GetCategoryResponseVm>>
            {
                Data = await _categoryService.GetPagedCategoriesAsync(getCategoriesRequestVm)
            };
        }

        [HttpGet("api/category/{id:int:min(1)}")]
        public async Task<Response<GetCategoryResponseVm>> GetCategoryById([FromQuery] GetCategoryRequestVm getCategoryRequestVm)
        {
            var category = await _categoryService.GetCategoryAsync(getCategoryRequestVm);
            return new Response<GetCategoryResponseVm>
            {
                Data = category
            };
        }

        [HttpPost("api/category/add")]
        public async Task<BaseResponse> AddCategory([FromBody] AddCategoryRequestVm addCategoryViewModel)
        {
            var generalResponse = await _categoryService.AddCategoryAsync(addCategoryViewModel);
            return new BaseResponse
            {
                ErrorCode = (int)ErrorCodes.Success
            };
        }

        [HttpPut("api/category/update")]
        public async Task<BaseResponse> EditCategory([FromBody] EditCategoryRequestVm editCategoryRequestVm)
        {
            var generalResponse = await _categoryService.EditCategoryAsync(editCategoryRequestVm);
            return new BaseResponse
            {
                ErrorCode = (int)ErrorCodes.Success
            };
        }

        [HttpDelete("api/category/delete/{id:int:min(1)}")]
        public async Task<Response<GetCategoryResponseVm>> DeleteCategoryById([FromQuery] DeleteCategoryRequestVm deleteCategoryRequestVm)
        {
            var deleteCategory = await _categoryService.DeleteCategoryAsync(deleteCategoryRequestVm);
            return new Response<GetCategoryResponseVm>
            {
                Data = deleteCategory
            };
        }

    }
}
