using System.Threading.Tasks;
using WEBAPI.Common.ViewModels;
using WEBAPI.Infrastructure.Query.Response;
using WEBAPI.Service.ViewModels;

namespace WEBAPI.Service.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<int> GetCategoriesCountAsync();
        Task<PagedList<GetCategoryResponseVm>> GetPagedCategoriesAsync(GetCategoriesRequestVm getCategoriesRequestVm);
        Task<GetCategoryResponseVm> GetCategoryAsync(GetCategoryRequestVm getCategoryRequestVm);
        Task<bool?> AddCategoryAsync(AddCategoryRequestVm addCategoryRequestVm);
        Task<bool?> EditCategoryAsync(EditCategoryRequestVm editCategoryRequestVm);
        Task<GetCategoryResponseVm> DeleteCategoryAsync(DeleteCategoryRequestVm deleteCategoryRequestVm);
    }
}
