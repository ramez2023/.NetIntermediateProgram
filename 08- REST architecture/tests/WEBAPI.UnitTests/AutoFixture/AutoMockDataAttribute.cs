using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace WEBAPI.UnitTests.AutoFixture;

[AttributeUsage(AttributeTargets.Method)]
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


        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));

        fixture.Behaviors.Add(new OmitOnRecursionBehavior());


        return fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }
}