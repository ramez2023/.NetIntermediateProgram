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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(
            IMapper mapper,
            IProductRepository productRepository, 
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetProductsCountAsync()
        {
            return await _productRepository.GetCountAsync();
        }
        public async Task<PagedList<GetProductResponseVm>> GetPagedProductsAsync(GetProductsRequestVm getProductsRequestVm)
        {
            var getProductsQueryDto = _mapper.Map<GetProductsQueryDto>(getProductsRequestVm);
            return await _productRepository.GetPagedProductsAsync(getProductsQueryDto);
        }
        public async Task<GetProductResponseVm> GetProductAsync(GetProductRequestVm getProductRequestVm)
        {
            if (getProductRequestVm.Id <= 0)
                throw new InvalidParameterException(nameof(getProductRequestVm.Id), getProductRequestVm.Id.ToString());

            var getProductQueryDto = _mapper.Map<GetProductQueryDto>(getProductRequestVm);
            Product product = await _productRepository.GetProductAsync(getProductQueryDto);

            if (product == null)
                throw new ProductNotFoundException(getProductRequestVm.Id);

            var response = _mapper.Map<GetProductResponseVm>(product);
            return response;
        }
        public async Task<bool?> AddProductAsync(AddProductRequestVm addProductViewModel)
        {
            addProductViewModel.Validate(() => new AddProductRequestVmValidator());
            var product = _mapper.Map<Product>(addProductViewModel);

            await _productRepository.AddAsync(product);
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        public async Task<bool?> EditProductAsync(EditProductRequestVm editProductRequestVm)
        {
            editProductRequestVm.Validate(() => new EditProductRequestVmValidator());
            Product product = await _productRepository.GetByIdAsync(editProductRequestVm.Id);
            if (product == null)
                throw new ProductNotFoundException(editProductRequestVm.Id);

            product.Name = editProductRequestVm.Name;
            product.Price = editProductRequestVm.Price;
            product.CategoryId = editProductRequestVm.CategoryId;
            product.Description = editProductRequestVm.Description;
            product.IsAvailable = editProductRequestVm.IsAvailable;
            product.Sku = editProductRequestVm.Sku;

            _productRepository.Update(product);
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        public async Task<GetProductResponseVm> DeleteProductAsync(DeleteProductRequestVm deleteProductRequestVm)
        {
            if (deleteProductRequestVm.Id <= 0)
                throw new InvalidParameterException(nameof(deleteProductRequestVm.Id), deleteProductRequestVm.Id.ToString());

            Product product = await _productRepository.GetByIdAsync(deleteProductRequestVm.Id);
            if (product == null)
                throw new ProductNotFoundException(deleteProductRequestVm.Id);

            await _productRepository.DeleteByIdAsync(deleteProductRequestVm.Id);
            await _unitOfWork.CommitTransactionAsync();
            
            var response = _mapper.Map<GetProductResponseVm>(product);
            return response;
        }

    }
}
