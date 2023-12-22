using WEBAPI.Common.Enums;

namespace WEBAPI.Infrastructure.Query.Request
{
    public class GetCategoriesQueryDto : BaseQueryDto
    {
        public string Name { get; set; }
        public CategorySortAttributes SortBy { get; set; } = CategorySortAttributes.CreateDate;
    }
}
