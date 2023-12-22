using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace WEBAPI.IntegrationTests.AutoFixture;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class AutoMockDataAttribute : AutoDataAttribute
{

    public AutoMockDataAttribute(params string[] values)
        : base(() => CreateFixture(values))
    {

    }

    public static IFixture CreateFixture(params string[] values)
    {
        Fixture fixture = new Fixture();
        fixture.Customize(new CompositeCustomization(
           new StringCustomization(),
           new CategoryCustomization(),
           new ProductCustomization()           
           ));

        return fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }
}