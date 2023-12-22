using WEBAPI.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI.Service.ViewModels
{
    public class DeleteProductRequestVm : BaseRequestVm
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }
}
