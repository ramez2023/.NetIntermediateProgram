using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime;
using System;
using WEBAPI.Domain.Entities;

namespace WEBAPI.Infrastructure.Query.Response
{
    public class GetCategoryResponseVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreateDate { get; set; }

    }


    public static class AsGetCategoryResponseVm
    {
        public static Expression<Func<Category, GetCategoryResponseVm>> Select()
        {
            return e => new GetCategoryResponseVm()
            {
                Id = e.Id,
                Name = e.Name,
                CreateDate = e.CreateDate.ToShortDateString()
            };
        }
    }

}
