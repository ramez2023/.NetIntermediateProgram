using System.Collections.Generic;
using System.Threading.Tasks;
using WEBAPI.Common.ViewModels;
using WEBAPI.Domain.Entities;
using WEBAPI.Infrastructure.Query.Request;
using WEBAPI.Infrastructure.Query.Response;

namespace WEBAPI.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<PagedList<GetProductResponseVm>> GetPagedProductsAsync(GetProductsQueryDto getProductsQueryDto);
        Task<Product> GetProductAsync(GetProductQueryDto getProductQueryDto);

    }
}