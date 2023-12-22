using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using WEBAPI.Common.Enums;
using WEBAPI.Common.ViewModels;
using WEBAPI.Infrastructure.Query.Response;
using WEBAPI.IntegrationTests.AutoFixture;
using WEBAPI.IntegrationTests.Extensions;
using WEBAPI.IntegrationTests.Mock;
using WEBAPI.IntegrationTests.Services.Interfaces;
using WEBAPI.Service.ViewModels;
using Xunit;

namespace WEBAPI.IntegrationTests.Features.CategoryTests
{
    [Collection("Services collection")]
    public class CategoryPositiveTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly ICategoryTestService _categoryTestService;
        private readonly HttpClient _client;

        public CategoryPositiveTest(CustomWebApplicationFactory<Program> factory)
        {
            using var scope = factory.Services.CreateScope();
            _categoryTestService = scope.ServiceProvider.GetRequiredService<ICategoryTestService>();
            _client = factory.CreateClient();
        }

        [Theory]
        [AutoMockData]
        public async Task GetList_SearchWithoutAnyParam_Success200Ok(
            List<AddCategoryRequestVm> categories)
        {
            //Arrange
            await AddCategoriesTestData(categories);

            //Act
            var response = await _categoryTestService.GetAll(new(), _client);
            var responseObject = await response.Content.ReadAsJsonAsync<Response<TestPagedList<GetCategoryResponseVm>>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject?.Data.List.Should().NotBeNull();
            responseObject?.Data.TotalCount.Should().BeGreaterThan(0);

            await CleanTestData(responseObject?.Data.List);
        }

        [Theory]
        [AutoMockData]
        public async Task GetList_SearchWithName_Success200Ok(
            List<AddCategoryRequestVm> categories)
        {
            //Arrange
            var keyWord = Guid.NewGuid();
            await AddCategoriesTestData(categories, keyWord);

            //Act
            var response = await _categoryTestService.GetAll(new GetCategoriesRequestVm()
            {
                Name = keyWord.ToString(),
            }, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<Response<TestPagedList<GetCategoryResponseVm>>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject?.Data.List.Should().NotBeNull();
            responseObject?.Data.TotalCount.Should().Be(categories.Count);

            await CleanTestData(responseObject?.Data.List);
        }

        [Theory]
        [AutoMockData]
        public async Task GetList_SearchWithNamePagination_Success200Ok(
            List<AddCategoryRequestVm> categories)
        {
            //Arrange
            var keyWord = Guid.NewGuid();
            var pageNumber = 1;
            var pageSize = 2;
            await AddCategoriesTestData(categories, keyWord);

            //Act
            var response = await _categoryTestService.GetAll(new GetCategoriesRequestVm()
            {
                Name = keyWord.ToString(),
                PageNumber = pageNumber,
                PageSize = pageSize
            }, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<Response<TestPagedList<GetCategoryResponseVm>>>();

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
            AddCategoryRequestVm category)
        {
            //Arrange
            category.Name = Guid.NewGuid().ToString();

            //Act
            var response = await _categoryTestService.Create(category, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<BaseResponse>();

            //Assert
            var categoryInDb = await GetCategoryTestData(category.Name);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject?.ErrorCode.Should().Be((int)ErrorCodes.Success);
            categoryInDb.Name.Should().Be(category.Name);

            await CleanTestData(new List<GetCategoryResponseVm>() { categoryInDb });
        }
        
        [Theory]
        [AutoMockData]
        public async Task Update_ValidData_Success200Ok(
            AddCategoryRequestVm category)
        {
            //Arrange
            category.Name = Guid.NewGuid().ToString();
            await _categoryTestService.Create(category, _client);

            //Act
            var categoryInDb = await GetCategoryTestData(category.Name);
            var response = await _categoryTestService.Update(new EditCategoryRequestVm
            {
                Name = categoryInDb.Name + "Updated",
                Id = categoryInDb.Id
            }, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<BaseResponse>();

            //Assert
            var categoryUpdatedInDb = await GetCategoryTestData(category.Name);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject?.ErrorCode.Should().Be((int)ErrorCodes.Success);
            categoryUpdatedInDb.Name.Should().Contain("Updated");

            await CleanTestData(new List<GetCategoryResponseVm>(){ categoryInDb });
        }

        [Theory]
        [AutoMockData]
        public async Task Delete_ValidData_Success200Ok(
            AddCategoryRequestVm category)
        {
            //Arrange
            category.Name = Guid.NewGuid().ToString();
            await _categoryTestService.Create(category, _client);

            //Act
            var categoryInDb = await GetCategoryTestData(category.Name);
            var response = await _categoryTestService.Delete(categoryInDb.Id, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<BaseResponse>();

            //Assert
            var categoryDeleteInDb = await GetCategoryTestData(category.Name);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseObject?.ErrorCode.Should().Be((int)ErrorCodes.Success);
            categoryDeleteInDb.Should().BeNull();

        }


        #region Helper
        private async Task AddCategoriesTestData(List<AddCategoryRequestVm> categories, Guid guid = default)
        {
            if (guid == default)
                guid = Guid.NewGuid();

            for (int i = 0; i < categories.Count; i++)
            {
                categories[i].Name = $"{guid}-Name-Category{i + 1}";
                await _categoryTestService.Create(categories[i], _client);
            }
        }
        private async Task<GetCategoryResponseVm?> GetCategoryTestData(string categoryName)
        {
            var response = await _categoryTestService.GetAll(new GetCategoriesRequestVm() { Name = categoryName, }, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<Response<TestPagedList<GetCategoryResponseVm>>>();

            return responseObject?.Data?.List?.FirstOrDefault();
        }
        private async Task CleanTestData(List<GetCategoryResponseVm> categories)
        {
            foreach (var category in categories)
            {
                await _categoryTestService.Delete(category.Id, _client);
            }
        }

        #endregion

    }
}
