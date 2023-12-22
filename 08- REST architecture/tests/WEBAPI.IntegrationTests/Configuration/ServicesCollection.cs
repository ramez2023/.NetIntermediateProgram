using Xunit;

namespace WEBAPI.IntegrationTests.Configuration
{
    [CollectionDefinition("Services collection")]
    public class ServicesCollection : ICollectionFixture<BaseTest>
    {
    }
}