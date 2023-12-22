using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WEBAPI.Common.Enums;
using WEBAPI.Common.Extensions;
using WEBAPI.Common.ViewModels;
using WEBAPI.Domain.Entities;
using WEBAPI.Infrastructure.Query.Request;
using WEBAPI.Infrastructure.Query.Response;
using WEBAPI.Infrastructure.Repositories.Interfaces;

namespace WEBAPI.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product, ApplicationContext>, IProductRepository
    {
        public ProductRepository(ApplicationContext context) : base(context)
        {
        }        
        
        public async Task<PagedList<GetProductResponseVm>> GetPagedProductsAsync(GetProductsQueryDto dto)
        {
            IQueryable<Product> query = _context.Products
                .Include(q => q.Category)
                .Where(q => !q.IsDeleted)
                .AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(dto.Name))
                query = query.Where(q => q.Name.Contains(dto.Name) || q.Description.Contains(dto.Name));

            if (!string.IsNullOrEmpty(dto.CategoryName))
                query = query.Where(q => q.Category.Name.Contains(dto.CategoryName));

            if (dto.CategoryId is > 0)
                query = query.Where(q => q.CategoryId == dto.CategoryId);

            query.OrderByCustom(Enum.GetName(typeof(ProductSortAttributes), dto.SortBy), dto.SortDirection);

            return await query.PaginateAsync(dto.PageNumber, dto.PageSize, AsGetProductResponseVm.Select());
        }

        public async Task<Product> GetProductAsync(GetProductQueryDto dto)
        {
            IQueryable<Product> query = _context.Products
                .Include(q => q.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(dto.Name))
                query = query.Where(q => q.Name.Contains(dto.Name) || q.Description.Contains(dto.Name));

            return await query.FirstOrDefaultAsync(s => s.Id == dto.Id && !s.IsDeleted);
        }

    }
}