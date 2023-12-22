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
using WEBAPI.Domain.Entities;

namespace WEBAPI.IntegrationTests.Features.ProductTests
{
    [Collection("Services collection")]
    public class ProductNegativeTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {

    }
}