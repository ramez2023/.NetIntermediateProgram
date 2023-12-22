using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime;
using System;
using WEBAPI.Domain.Entities;

namespace WEBAPI.Infrastructure.Query.Response
{
    public class GetProductResponseVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string CategoryName { get; set; }

    }


    public static class AsGetProductResponseVm
    {
        public static Expression<Func<Product, GetProductResponseVm>> Select()
        {
            return e => new GetProductResponseVm()
            {
                Id = e.Id,
                Name = e.Name,
                Sku = e.Sku,
                Description = e.Description,
                Price = e.Price,
                IsAvailable = e.IsAvailable,
                CategoryName = e.Category.Name,
            };
        }
    }

}
