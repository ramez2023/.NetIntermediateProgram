using System;
using System.Collections.Generic;
using System.Text;
using WEBAPI.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI.Service.ViewModels
{
    public class GetProductRequestVm : BaseRequestVm
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }

        [FromQuery(Name = "Name")]
        public string Name { get; set; }
    }
}
