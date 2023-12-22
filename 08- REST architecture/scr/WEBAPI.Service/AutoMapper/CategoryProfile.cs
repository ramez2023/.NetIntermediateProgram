using AutoMapper;
using WEBAPI.Domain.Entities;
using WEBAPI.Infrastructure.Query.Request;
using WEBAPI.Infrastructure.Query.Response;
using WEBAPI.Service.ViewModels;

namespace WEBAPI.Service.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<AddCategoryRequestVm, Category>();

            CreateMap<GetCategoriesRequestVm, GetCategoriesQueryDto>();
            CreateMap<GetCategoryRequestVm, GetCategoryQueryDto>();

            CreateMap<Category, GetCategoryResponseVm>().ReverseMap();
        }
    }
}
