using AutoFixture;
using WEBAPI.Domain.Entities;

namespace WEBAPI.IntegrationTests.AutoFixture;
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
                .With(t => t.CreateBy, "IntegrationTests")
                .With(t => t.CreateDate, DateTime.UtcNow)
                .With(t => t.IsDeleted, false);
        });
    }
}