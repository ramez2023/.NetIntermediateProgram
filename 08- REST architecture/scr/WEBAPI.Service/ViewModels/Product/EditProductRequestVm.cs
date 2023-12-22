using System;
using System.Collections.Generic;
using System.Text;
using WEBAPI.Common.ViewModels;

namespace WEBAPI.Service.ViewModels
{
    public class EditProductRequestVm : BaseRequestVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
    }
}
