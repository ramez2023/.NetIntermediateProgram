using WEBAPI.Common.Enums;

namespace WEBAPI.Infrastructure.Query.Request
{
    public class BaseQueryDto
    {
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
