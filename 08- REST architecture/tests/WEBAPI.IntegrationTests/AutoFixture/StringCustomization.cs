using AutoFixture;

namespace WEBAPI.IntegrationTests.AutoFixture;
public class StringCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<string>(x => x.FromFactory(() => Guid.NewGuid().ToString()));
    }
}
