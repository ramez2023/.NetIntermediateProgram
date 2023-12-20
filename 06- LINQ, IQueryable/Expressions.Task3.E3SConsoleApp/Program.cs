using System.Linq;
using System.Linq.Expressions;
using Expressions.Task3.E3SQueryProvider.SqlQueryProvider;

namespace Expressions.Task3.E3SConsoleApp;

class Program
{
    static void Main()
    {
        var queryProvider = new E3SLinqSqlProvider("Server=.;Database=SqlProviderTaskDb;Trusted_Connection=True;MultipleActiveResultSets=true");

        TestReturnList(queryProvider);
        TestReturnList_WithWhereCondition(queryProvider);
        Console.ReadLine();
    }

    static void TestReturnList(E3SLinqSqlProvider queryProvider)
    {

        E3SSqlQuery<Person> query = new E3SSqlQuery<Person>(queryProvider);
        var resultList01 = query.ToList();
        var resultList02 = query.Where(p => p.Age > 10 && p.Id > 5).ToList();
        var resultList03 = query.Where(p => p.Id < 50 && p.Name == "Person-06").ToList();

        Console.WriteLine("---------------- resultList01 -------------------");
        PrintResultSet(resultList01);

        Console.WriteLine("---------------- resultList02 -------------------");
        PrintResultSet(resultList02);

        Console.WriteLine("---------------- resultList03 -------------------");
        PrintResultSet(resultList03);

    }

    static void TestReturnList_WithWhereCondition(E3SLinqSqlProvider queryProvider)
    {
        E3SSqlQuery<Person> sqlQuery = new E3SSqlQuery<Person>(queryProvider);

        var query = sqlQuery.Where(p => p.Age > 10 && p.Id > 5);
        query = query.Where(p => p.Id < 50);

        var resultList = query.ToList();


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
    static void PrintResultSet(IEnumerable<Person> resultList)
    {
        var enumerable = resultList as Person[] ?? resultList.ToArray();
        if (enumerable.Any())
        {
            foreach (var result in enumerable)
            {
                Console.WriteLine($"Result: {result.Id}, {result.Name}");
            }
        }
        else
        {
            Console.WriteLine("No result found.");
        }
        Console.WriteLine();
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine();

    }
}