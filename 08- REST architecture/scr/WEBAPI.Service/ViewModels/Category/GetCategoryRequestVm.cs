using WEBAPI.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI.Service.ViewModels
{
    public class GetCategoryRequestVm : BaseRequestVm
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }

        [FromQuery(Name = "Name")]
        public string Name { get; set; }
    }
}
