using System.Linq.Expressions;
using Expressions.Task3.E3SQueryProvider.QueryProvider;

namespace Expressions.Task3.E3SConsoleApp;

class Program
{
    static void Main()
    {
        TestReturnListObject();
        TestReturnOneObject();
        Console.ReadLine();
    }


    static void TestReturnListObject()
    {
        var queryProvider = new E3SLinqSqlProvider("Server=.;Database=SqlProviderTaskDb;Trusted_Connection=True;MultipleActiveResultSets=true");

        Expression<Func<Person, bool>> filter = entity => entity.Id > 1 && entity.Age > 15;
        var resultList = queryProvider.Execute<IEnumerable<Person>>(filter);

        if (resultList.Any())
        {
            foreach (var result in resultList)
            {
                Console.WriteLine($"Result: {result.Id}, {result.Name}");
            }
        }
        else
        {
            Console.WriteLine("No result found.");
        }
    }

    static void TestReturnOneObject()
    {
        var queryProvider = new E3SLinqSqlProvider("Server=.;Database=SqlProviderTaskDb;Trusted_Connection=True;MultipleActiveResultSets=true");

        Expression<Func<Person, bool>> filter = entity => entity.Id > 1 && entity.Age > 15;

        if (queryProvider.Execute(filter) is Person result)
        {
            Console.WriteLine($"Result: {result.Id}, {result.Name}");
        }
        else
        {
            Console.WriteLine("No result found.");
        }
    }
}