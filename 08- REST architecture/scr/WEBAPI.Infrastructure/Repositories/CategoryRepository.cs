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
    public class CategoryRepository : GenericRepository<Category, ApplicationContext>, ICategoryRepository
    {
        public CategoryRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<PagedList<GetCategoryResponseVm>> GetPagedCategoriesAsync(GetCategoriesQueryDto dto)
        {
            IQueryable<Category> query = _context.Categories
                                                    .Where(q => !q.IsDeleted)
                                                    .AsNoTracking().AsQueryable();
            if(!string.IsNullOrEmpty(dto.Name))
                query = query.Where(q => q.Name.Contains(dto.Name));

            query.OrderByCustom(Enum.GetName(typeof(CategorySortAttributes), dto.SortBy), dto.SortDirection);

            return await query.PaginateAsync(dto.PageNumber, dto.PageSize, AsGetCategoryResponseVm.Select());
        }

        public async Task<Category> GetCategoryAsync(GetCategoryQueryDto dto)
        {
            IQueryable<Category> query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(dto.Name))
                query = query.Where(q => q.Name.Contains(dto.Name));

            return await query.FirstOrDefaultAsync(s => s.Id == dto.Id && !s.IsDeleted);
        }

    }
}