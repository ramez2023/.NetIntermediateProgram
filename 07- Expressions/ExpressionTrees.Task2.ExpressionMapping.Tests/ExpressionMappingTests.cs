using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests;

[TestClass]
public class ExpressionMappingTests
{
    // todo: add as many test methods as you wish, but they should be enough to cover basic scenarios of the mapping generator

    [TestMethod]
    public void TestMethod1()
    {
        var mapGenerator = new MappingGenerator();
        var mapper = mapGenerator.Generate<Foo, Bar>();

        var res = mapper.Map(new Foo());
    }

    [TestMethod]
    public void TestMapping()
    {
        // Arrange
        var fieldMappings = new Dictionary<string, string>
        {
            { "Id", "Identifier" },
            { "Name", "FullName" }
        };

        var mapGenerator = new MappingGenerator();
        var mapper = mapGenerator.Generate<Foo, Bar>(fieldMappings);

        // Act
        var fooInstance = new Foo { Id = 1, Name = "John Doe" };
        var barInstance = mapper.Map(fooInstance);

        // Assert
        Assert.AreEqual(fooInstance.Id, barInstance.Identifier);
        Assert.AreEqual(fooInstance.Name, barInstance.FullName);
    }

}