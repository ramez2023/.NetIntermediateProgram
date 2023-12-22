using Microsoft.Extensions.DependencyInjection;
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

namespace WEBAPI.IntegrationTests.Features.CategoryTests
{
    [Collection("Services collection")]
    public class CategoryNegativeTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly ICategoryTestService _categoryTestService;
        private readonly HttpClient _client;

        public CategoryNegativeTest(CustomWebApplicationFactory<Program> factory)
        {
            using var scope = factory.Services.CreateScope();
            _categoryTestService = scope.ServiceProvider.GetRequiredService<ICategoryTestService>();
            _client = factory.CreateClient();
        }

        [Theory]
        [AutoMockData]
        public async Task Add_InvalidData_Failed(
            AddCategoryRequestVm category)
        {
            //Arrange
            category.Name = null;

            //Act
            var response = await _categoryTestService.Create(category, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<BaseResponse>();

            //Assert
            responseObject?.ErrorCode.Should().NotBe((int)ErrorCodes.Success);
            responseObject?.ErrorMsg.Should().NotBeNull();
        }

        [Theory]
        [AutoMockData]
        public async Task Update_InvalidData_Failed(
            AddCategoryRequestVm category)
        {
            //Arrange
            category.Name = Guid.NewGuid().ToString();
            await _categoryTestService.Create(category, _client);

            //Act
            var categoryInDb = await GetCategoryTestData(category.Name);
            var response = await _categoryTestService.Update(new EditCategoryRequestVm
            {
                Name = null,
                Id = categoryInDb.Id
            }, _client);
            var responseObject = await response.Content.ReadAsJsonAsync<BaseResponse>();

            //Assert
            var categoryUpdatedInDb = await GetCategoryTestData(category.Name);
            responseObject?.ErrorCode.Should().NotBe((int)ErrorCodes.Success);
            responseObject?.ErrorMsg.Should().NotBeNull();
            categoryUpdatedInDb.Name.Should().Contain(category.Name);

            await CleanTestData(new List<GetCategoryResponseVm>() { categoryInDb });
        }

        [Theory]
        [AutoMockData]
        public async Task Delete_ValidData_Success200Ok(
            AddCategoryRequestVm category)
        {
            //Arrange

            //Act
            var response = await _categoryTestService.Delete(new Random().Next(), _client);
            var responseObject = await response.Content.ReadAsJsonAsync<BaseResponse>();

            //Assert
            responseObject?.ErrorCode.Should().NotBe((int)ErrorCodes.Success);
            responseObject?.ErrorMsg.Should().NotBeNull();
        }


        #region Helper
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