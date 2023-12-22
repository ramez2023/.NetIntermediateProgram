using System;
using WEBAPI.Common.Enums;

namespace WEBAPI.Infrastructure.Query.Request
{
    public class GetProductQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
