using WEBAPI.Common.Enums;
using WEBAPI.Common.ViewModels;

namespace WEBAPI.Service.ViewModels
{
    public class GetCategoriesRequestVm : BasePagedRequestVm
    {
        public string Name { get; set; }
        public CategorySortAttributes SortBy { get; set; } = CategorySortAttributes.CreateDate;
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;


    }
}
