using AutoFixture;
using WEBAPI.Domain.Entities;

namespace WEBAPI.IntegrationTests.AutoFixture;
public class CategoryCustomization : ICustomization
{
    public CategoryCustomization()
    {
       
    }
    public void Customize(IFixture fixture)
    {
        fixture.Customize<Category>(composer =>
        {
            return composer                
                .With(t => t.CreateBy, "IntegrationTests")
                .With(t => t.CreateDate, DateTime.UtcNow)
                .With(t => t.IsDeleted, false);
        });
    }
}