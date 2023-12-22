using AutoMapper;
using WEBAPI.Domain.Entities;
using WEBAPI.Infrastructure.Query.Request;
using WEBAPI.Infrastructure.Query.Response;
using WEBAPI.Service.ViewModels;

namespace WEBAPI.Service.AutoMapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductRequestVm, Product>();

            CreateMap<GetProductsRequestVm, GetProductsQueryDto>();
            CreateMap<GetProductRequestVm, GetProductQueryDto>();

            CreateMap<Product, GetProductResponseVm>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        }

    }
}
