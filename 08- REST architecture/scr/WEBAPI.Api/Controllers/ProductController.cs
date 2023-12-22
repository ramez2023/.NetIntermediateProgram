using WEBAPI.Common.ViewModels;
using WEBAPI.Service.Services.Interfaces;
using WEBAPI.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;
using WEBAPI.Common.Enums;
using WEBAPI.Infrastructure.Query.Response;

namespace WEBAPI.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("api/products/count")]
        public async Task<Response<int>> GetProductsCount([FromQuery] BaseRequestVm requestVm)
        {
            return new Response<int>
            {
                Data = await _productService.GetProductsCountAsync()
            };
        }

        [HttpGet("api/products")]
        public async Task<Response<PagedList<GetProductResponseVm>>> GetPagedProducts([FromQuery] GetProductsRequestVm getProductsRequestVm)
        {
            return new Response<PagedList<GetProductResponseVm>>
            {
                Data = await _productService.GetPagedProductsAsync(getProductsRequestVm)
            };
        }

        [HttpGet("api/product/{id}")]
        public async Task<Response<GetProductResponseVm>> GetProductById([FromQuery] GetProductRequestVm getProductRequestVm)
        {
            var product = await _productService.GetProductAsync(getProductRequestVm);
            return new Response<GetProductResponseVm>
            {
                Data = product
            };
        }

        [HttpPost("api/product/add")]
        public async Task<BaseResponse> AddProduct([FromBody] AddProductRequestVm addProductViewModel)
        {
            var generalResponse = await _productService.AddProductAsync(addProductViewModel);
            return new BaseResponse
            {
                ErrorCode = (int)ErrorCodes.Success
            };
        }

        [HttpPut("api/product/update")]
        public async Task<BaseResponse> EditProduct([FromBody] EditProductRequestVm editProductRequestVm)
        {
            var generalResponse = await _productService.EditProductAsync(editProductRequestVm);
            return new BaseResponse
            {
                ErrorCode = (int)ErrorCodes.Success
            };
        }

        [HttpDelete("api/product/delete/{id}")]
        public async Task<Response<GetProductResponseVm>> DeleteProductById([FromQuery] DeleteProductRequestVm deleteProductRequestVm)
        {
            var deleteProduct = await _productService.DeleteProductAsync(deleteProductRequestVm);
            return new Response<GetProductResponseVm>
            {
                Data = deleteProduct
            };
        }

    }
}
