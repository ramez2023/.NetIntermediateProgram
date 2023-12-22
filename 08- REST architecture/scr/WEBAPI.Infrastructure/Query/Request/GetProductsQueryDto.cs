using System;
using WEBAPI.Common.Enums;
using WEBAPI.Infrastructure.Query.Request;

namespace WEBAPI.Infrastructure.Query.Request
{
    public class GetProductsQueryDto : BaseQueryDto
    {
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int? CategoryId { get; set; }
        public ProductSortAttributes SortBy { get; set; } = ProductSortAttributes.Name;
    }
}
