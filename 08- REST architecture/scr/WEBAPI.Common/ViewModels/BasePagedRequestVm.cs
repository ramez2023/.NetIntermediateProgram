using WEBAPI.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI.Common.ViewModels
{
    public class BasePagedRequestVm
    {
        [FromQuery(Name = "PageSize")]
        public int PageSize { get; set; } = 10;


        [FromQuery(Name = "PageNumber")]
        public int PageNumber { get; set; } = 1;

    }
}
