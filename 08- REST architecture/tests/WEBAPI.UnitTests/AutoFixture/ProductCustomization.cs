using AutoFixture;
using WEBAPI.Domain.Entities;

namespace WEBAPI.UnitTests.AutoFixture;
public class ProductCustomization : ICustomization
{
    public ProductCustomization()
    {
       
    }
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Product>(composer =>
        {
            return composer                
                .With(t => t.CreateBy, "UnitTests")
                .With(t => t.CreateDate, DateTime.UtcNow)
                .With(t => t.IsDeleted, false);
        });
    }
}