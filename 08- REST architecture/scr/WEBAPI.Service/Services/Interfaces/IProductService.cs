using System.Threading.Tasks;
using WEBAPI.Common.ViewModels;
using WEBAPI.Infrastructure.Query.Response;
using WEBAPI.Service.ViewModels;

namespace WEBAPI.Service.Services.Interfaces
{
    public interface IProductService
    {
        Task<int> GetProductsCountAsync();
        Task<PagedList<GetProductResponseVm>> GetPagedProductsAsync(GetProductsRequestVm getProductsRequestVm);
        Task<GetProductResponseVm> GetProductAsync(GetProductRequestVm getProductRequestVm);
        Task<bool?> AddProductAsync(AddProductRequestVm addProductRequestVm);
        Task<bool?> EditProductAsync(EditProductRequestVm editProductRequestVm);
        Task<GetProductResponseVm> DeleteProductAsync(DeleteProductRequestVm deleteProductRequestVm);
    }
}
