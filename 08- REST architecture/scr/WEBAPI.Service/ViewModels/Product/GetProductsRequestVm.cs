using System;
using System.Collections.Generic;
using System.Text;
using WEBAPI.Common.Enums;
using WEBAPI.Common.ViewModels;

namespace WEBAPI.Service.ViewModels
{
    public class GetProductsRequestVm : BasePagedRequestVm
    {
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public ProductSortAttributes SortBy { get; set; } = ProductSortAttributes.Name;
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;
    }
}
