using System;
using System.Collections.Generic;
using System.Text;
using WEBAPI.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI.Service.ViewModels
{
    public class DeleteCategoryRequestVm : BaseRequestVm
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }
}
