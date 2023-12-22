using Microsoft.Extensions.DependencyInjection;
using System.Net;
using FluentAssertions;
using WEBAPI.Common.Enums;
using WEBAPI.Common.ViewModels;
using WEBAPI.Infrastructure.Query.Response;
using WEBAPI.IntegrationTests.AutoFixture;
using WEBAPI.IntegrationTests.Extensions;
using WEBAPI.IntegrationTests.Mock;
using WEBAPI.IntegrationTests.Services.Interfaces;
using WEBAPI.Service.ViewModels;
using Xunit;

namespace WEBAPI.IntegrationTests.Features.ProductTests
{
    [Collection("Services collection")]
    public class ProductPositiveTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly ICategoryTestService _categoryTestService;
        private readonly IProductTestService _productTestService;
        private readonly HttpClient _client;

        public ProductPositiveTest(CustomWebApplicationFactory<Program> factory)
        {
            using var scope = factory.Services.CreateScope();
            _categoryTestService = scope.ServiceProvider.GetRequiredService<ICategoryTestService>();
            _productTestService = scope.ServiceProvider.GetRequiredService<IProductTestService>();
            _client = factory.CreateClient();
        }

        [Theory]
        [AutoMockData]
        public async Task GetList_SearchWithCategoryName_Success200Ok(
            List<AddProductRequestVm> products,
            string categoryName)
        {
            //Arrange
            var keyWord = Guid.NewGuid();
            await AddProductWithCategoryTestData(products, categoryName, keyWord);

            //Act
            var response = await _productTestService.GetAll(new GetProductsRequestVm
            {
                Name = keyWord.ToString(),
                CategoryName = categoryName
            }, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<Response<TestPagedList<GetProductResponseVm>>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject?.Data.List.Select(p => p.CategoryName).Should().Contain(categoryName);
            responseObject?.Data.TotalCount.Should().Be(products.Count);

            await CleanTestData(responseObject?.Data.List);
        }

        [Theory]
        [AutoMockData]
        public async Task GetList_SearchWithCategoryId_Success200Ok(
            List<AddProductRequestVm> products,
            string categoryName)
        {
            //Arrange
            var keyWord = Guid.NewGuid();
            await AddProductWithCategoryTestData(products, categoryName, keyWord);
            var categoryInDb = await GetCategoryTestData(categoryName);

            //Act
            var response = await _productTestService.GetAll(new GetProductsRequestVm
            {
                Name = keyWord.ToString(),
                CategoryId = categoryInDb.Id
            }, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<Response<TestPagedList<GetProductResponseVm>>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject?.Data.List.Select(p => p.Name).Should().Contain(products.First().Name);
            responseObject?.Data.TotalCount.Should().Be(products.Count);

            await CleanTestData(responseObject?.Data.List);
        }

        [Theory]
        [AutoMockData]
        public async Task GetList_SearchWithNamePagination_Success200Ok(
            List<AddProductRequestVm> products,
            string categoryName)
        {
            //Arrange
            var keyWord = Guid.NewGuid();
            var pageNumber = 1;
            var pageSize = 2;
            await AddProductWithCategoryTestData(products, categoryName, keyWord);

            //Act
            var response = await _productTestService.GetAll(new GetCategoriesRequestVm()
            {
                Name = keyWord.ToString(),
                PageNumber = pageNumber,
                PageSize = pageSize
            }, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<Response<TestPagedList<GetProductResponseVm>>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject?.Data.List.Should().NotBeNull();
            responseObject?.Data.TakeCount.Should().Be(pageSize);
            responseObject?.Data.SkipCount.Should().Be(pageNumber - 1);

            await CleanTestData(responseObject?.Data.List);
        }

        [Theory]
        [AutoMockData]
        public async Task Add_ValidData_Success200Ok(
            AddProductRequestVm product,
            string categoryName)
        {
            //Arrange
            await _categoryTestService.Create(new AddCategoryRequestVm { Name = categoryName, }, _client);
            var categoryInDb = await GetCategoryTestData(categoryName);
            product.CategoryId = categoryInDb.Id;

            //Act
            var response = await _productTestService.Create(product, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<BaseResponse>();

            //Assert
            var productInDb = await GetProductTestData(product.Name);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject?.ErrorCode.Should().Be((int)ErrorCodes.Success);
            productInDb.Name.Should().Be(product.Name);

            await CleanTestData(new List<GetProductResponseVm>() { productInDb });
        }

        [Theory]
        [AutoMockData]
        public async Task Update_ValidData_Success200Ok(
            AddProductRequestVm product,
            string categoryName)
        {
            //Arrange
            await _categoryTestService.Create(new AddCategoryRequestVm { Name = categoryName, }, _client);
            var categoryInDb = await GetCategoryTestData(categoryName);
            product.CategoryId = categoryInDb.Id;
            await _productTestService.Create(product, _client);
            var productUpdatedInDb = await GetProductTestData(product.Name);

            //Act
            var response = await _productTestService.Update(new EditProductRequestVm()
            {
                Id = productUpdatedInDb.Id,
                Name = product.Name + "Updated",
                CategoryId = product.CategoryId,
                Price = product.Price + 20
            }, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<BaseResponse>();


            //Assert
            productUpdatedInDb = await GetProductTestData(product.Name);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject?.ErrorCode.Should().Be((int)ErrorCodes.Success);
            productUpdatedInDb.Name.Should().Contain("Updated");
            productUpdatedInDb.Price.Should().BeGreaterThan(20);

            await CleanTestData(new List<GetProductResponseVm>() { productUpdatedInDb });
        }

        [Theory]
        [AutoMockData]
        public async Task Delete_ValidData_Success200Ok(
            AddProductRequestVm product,
            string categoryName)
        {
            //Arrange
            await _categoryTestService.Create(new AddCategoryRequestVm { Name = categoryName, }, _client);
            var categoryInDb = await GetCategoryTestData(categoryName);
            product.CategoryId = categoryInDb.Id;
            await AddProductWithCategoryTestData(new List<AddProductRequestVm>() { product }, categoryName);

            //Act
            var productInDb = await GetProductTestData(product.Name);
            var response = await _productTestService.Delete(productInDb.Id, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<BaseResponse>();

            //Assert
            var productDeleteInDb = await GetCategoryTestData(product.Name);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject?.ErrorCode.Should().Be((int)ErrorCodes.Success);
            productDeleteInDb.Should().BeNull();

        }


        #region Helper
        private async Task AddProductWithCategoryTestData(List<AddProductRequestVm> products, string categoryName, Guid guid = default)
        {
            if (guid == default)
                guid = Guid.NewGuid();

            var category = await _categoryTestService.Create(new AddCategoryRequestVm
            {
                Name = categoryName,
            }, _client);

            var categoryInDb = await GetCategoryTestData(categoryName);

            for (int i = 0; i < products.Count; i++)
            {
                products[i].Name = $"Name({guid})-Test-Product{i + 1}";
                products[i].CategoryId = categoryInDb.Id;
                await _productTestService.Create(products[i], _client);
            }
        }
        private async Task<GetCategoryResponseVm?> GetCategoryTestData(string categoryName)
        {
            var response = await _categoryTestService.GetAll(new GetCategoriesRequestVm { Name = categoryName, }, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<Response<TestPagedList<GetCategoryResponseVm>>>();

            return responseObject?.Data?.List?.FirstOrDefault();
        }
        private async Task<GetProductResponseVm?> GetProductTestData(string productName)
        {
            var response = await _productTestService.GetAll(new GetProductsRequestVm() { Name = productName, }, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<Response<TestPagedList<GetProductResponseVm>>>();

            return responseObject?.Data?.List?.FirstOrDefault();
        }
        private async Task CleanTestData(List<GetProductResponseVm> products)
        {
            var categoryInDb = await GetCategoryTestData(products.First().CategoryName);
            await _categoryTestService.Delete(categoryInDb.Id, _client);

            foreach (var product in products)
            {
                await _productTestService.Delete(product.Id, _client);
            }
        }

        #endregion

    }
}
