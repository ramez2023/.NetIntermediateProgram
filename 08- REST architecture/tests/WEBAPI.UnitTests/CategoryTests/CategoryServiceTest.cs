using Moq;
using AutoFixture.Xunit2;
using FluentAssertions;
using WEBAPI.Infrastructure.Repositories.Interfaces;
using WEBAPI.Service.ViewModels;
using WEBAPI.UnitTests.AutoFixture;
using Xunit;
using WEBAPI.Common.ViewModels;
using WEBAPI.Domain.Entities;
using WEBAPI.Infrastructure.Query.Response;
using WEBAPI.Infrastructure.Query.Request;
using WEBAPI.Service.Services;
using AutoMapper;
using WEBAPI.Service.AutoMapper;

namespace WEBAPI.UnitTests.CategoryTests
{
    public class CategoryServiceTest
    {
        private readonly IMapper _mapperMock;

        public CategoryServiceTest()
        {
            _mapperMock = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CategoryProfile());
            }).CreateMapper();
        }

        [Theory]
        [AutoMockData]
        public async Task GetCategoriesCountAsyncTest(
            int count,
            [Frozen] Mock<ICategoryRepository> categoryRepositoryMock,
            [Greedy] CategoryService categoryService)
        {
            // Arrange                        
            categoryRepositoryMock
                .Setup(cl => cl.GetCountAsync())
                .ReturnsAsync(count);

            // Act
            var result = await categoryService.GetCategoriesCountAsync();

            // Assert
            result.Should().Be(count);
        }

        [Theory]
        [AutoMockData]
        public async Task GetPagedCategoriesAsyncTest(
            GetCategoriesRequestVm getCategoriesRequestVm,
            PagedList<GetCategoryResponseVm> pagedList,
            [Frozen] Mock<ICategoryRepository> categoryRepositoryMock,
            [Greedy] CategoryService categoryService)
        {
            // Arrange                        
            categoryRepositoryMock
                .Setup(cl => cl.GetPagedCategoriesAsync(It.IsAny<GetCategoriesQueryDto>()))
                .ReturnsAsync(pagedList);

            // Act
            var result = await categoryService.GetPagedCategoriesAsync(getCategoriesRequestVm);

            // Assert
            result.Should().Be(pagedList);
        }

        [Theory]
        [AutoMockData]
        public async Task AddCategoryAsyncTest(
            AddCategoryRequestVm categoryRequestVm,
            [Frozen] Mock<ICategoryRepository> categoryRepositoryMock,
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Greedy] CategoryService categoryService)
        {
            // Arrange                        
            categoryRepositoryMock
                .Setup(cl => cl.AddAsync(It.IsAny<Category>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            unitOfWorkMock
                .Setup(cl => cl.CommitTransactionAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await categoryService.AddCategoryAsync(categoryRequestVm);

            // Assert
            unitOfWorkMock.Verify(c => c.CommitTransactionAsync(), Times.Once);
            result.Should().Be(true);
        }

        [Theory]
        [AutoMockData]
        public async Task EditCategoryAsyncTest(
            EditCategoryRequestVm editCategoryRequestVm,
            Category category,
            [Frozen] Mock<ICategoryRepository> categoryRepositoryMock,
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Greedy] CategoryService categoryService)
        {
            // Arrange
            editCategoryRequestVm.Id = category.Id;

            categoryRepositoryMock
                .Setup(cl => cl.GetByIdAsync(editCategoryRequestVm.Id))
                .ReturnsAsync(category);

            categoryRepositoryMock
                .Setup(cl => cl.Update(It.IsAny<Category>(), It.IsAny<string>()));

            unitOfWorkMock
                .Setup(cl => cl.CommitTransactionAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await categoryService.EditCategoryAsync(editCategoryRequestVm);

            // Assert
            unitOfWorkMock.Verify(c => c.CommitTransactionAsync(), Times.Once);
            result.Should().Be(true);
        }

        [Theory]
        [AutoMockData]
        public async Task DeleteCategoryAsyncTest(
            DeleteCategoryRequestVm categoryRequestVm,
            Category category,
            [Frozen] Mock<ICategoryRepository> categoryRepositoryMock,
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Greedy] CategoryService categoryService)
        {
            // Arrange
            categoryRequestVm.Id = category.Id;

            categoryRepositoryMock
                .Setup(cl => cl.GetByIdAsync(categoryRequestVm.Id))
                .ReturnsAsync(category);

            categoryRepositoryMock
                .Setup(cl => cl.DeleteByIdAsync(category.Id, It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            unitOfWorkMock
                .Setup(cl => cl.CommitTransactionAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await categoryService.DeleteCategoryAsync(categoryRequestVm);

            // Assert
            unitOfWorkMock.Verify(c => c.CommitTransactionAsync(), Times.Once);
            result.Id.Should().Be(category.Id);
        }
    }
}
