using System;
using System.Collections.Generic;
using System.Text;

namespace WEBAPI.Domain.Entities
{
    public class Category: BaseEntity
    {

        public virtual ICollection<Product> Products { get; set; }

    }
}
