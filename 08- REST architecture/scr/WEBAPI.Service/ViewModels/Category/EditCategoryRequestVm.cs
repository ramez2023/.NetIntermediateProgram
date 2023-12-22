using System;
using System.Collections.Generic;
using System.Text;
using WEBAPI.Common.ViewModels;

namespace WEBAPI.Service.ViewModels
{
    public class EditCategoryRequestVm : BaseRequestVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
