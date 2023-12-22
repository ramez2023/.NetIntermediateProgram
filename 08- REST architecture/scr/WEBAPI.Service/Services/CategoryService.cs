using WEBAPI.Domain.Entities;
using AutoMapper;
using WEBAPI.Service.ViewModels;
using System.Threading.Tasks;
using WEBAPI.Common.ViewModels;
using WEBAPI.Common.Exceptions.Business;
using WEBAPI.Infrastructure.Query.Request;
using WEBAPI.Infrastructure.Query.Response;
using WEBAPI.Infrastructure.Repositories.Interfaces;
using WEBAPI.Service.Services.Interfaces;
using WEBAPI.Service.Validators;

namespace WEBAPI.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(
            IMapper mapper,
            ICategoryRepository categoryRepository, 
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetCategoriesCountAsync()
        {
            return await _categoryRepository.GetCountAsync();
        }
        public async Task<PagedList<GetCategoryResponseVm>> GetPagedCategoriesAsync(GetCategoriesRequestVm getCategoriesRequestVm)
        {
            var getCategoriesQueryDto = _mapper.Map<GetCategoriesQueryDto>(getCategoriesRequestVm);
            return await _categoryRepository.GetPagedCategoriesAsync(getCategoriesQueryDto);
        }
        public async Task<GetCategoryResponseVm> GetCategoryAsync(GetCategoryRequestVm getCategoryRequestVm)
        {
            if (getCategoryRequestVm.Id <= 0)
                throw new InvalidParameterException(nameof(getCategoryRequestVm.Id), getCategoryRequestVm.Id.ToString());

            var getCategoryQueryDto = _mapper.Map<GetCategoryQueryDto>(getCategoryRequestVm);
            Category category = await _categoryRepository.GetCategoryAsync(getCategoryQueryDto);

            if (category == null)
                throw new CategoryNotFoundException(getCategoryRequestVm.Id);

            var response = _mapper.Map<GetCategoryResponseVm>(category);
            return response;
        }
        public async Task<bool?> AddCategoryAsync(AddCategoryRequestVm addCategoryViewModel)
        {
            addCategoryViewModel.Validate(() => new AddCategoryRequestVmValidator()); 
            var category = _mapper.Map<Category>(addCategoryViewModel);

            await _categoryRepository.AddAsync(category);
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        public async Task<bool?> EditCategoryAsync(EditCategoryRequestVm editCategoryRequestVm)
        {
            editCategoryRequestVm.Validate(() => new EditCategoryRequestVmValidator());
            Category category = await _categoryRepository.GetByIdAsync(editCategoryRequestVm.Id);
            if (category == null)
                throw new CategoryNotFoundException(editCategoryRequestVm.Id);

            category.Name = editCategoryRequestVm.Name;

            _categoryRepository.Update(category);
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        public async Task<GetCategoryResponseVm> DeleteCategoryAsync(DeleteCategoryRequestVm deleteCategoryRequestVm)
        {
            Category category = await _categoryRepository.GetByIdAsync(deleteCategoryRequestVm.Id);
            if (category == null)
                throw new CategoryNotFoundException(deleteCategoryRequestVm.Id);

            await _categoryRepository.DeleteByIdAsync(deleteCategoryRequestVm.Id);
            await _unitOfWork.CommitTransactionAsync();
            
            var response = _mapper.Map<GetCategoryResponseVm>(category);
            return response;
        }

    }
}
