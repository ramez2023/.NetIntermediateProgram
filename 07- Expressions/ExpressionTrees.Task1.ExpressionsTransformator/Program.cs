/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            // todo: feel free to add your code here
            TestIncDecExpressionVisitor();
            TestLambdaParameterReplacementWithTwoParams();
            TestLambdaParameterReplacementWithThreeParams();

            Console.ReadLine();
        }

        static void TestIncDecExpressionVisitor()
        {
            ParameterExpression variable = Expression.Parameter(typeof(int), "x");
            BinaryExpression incrementExpression = Expression.Add(variable, Expression.Constant(1));
            BinaryExpression decrementExpression = Expression.Subtract(variable, Expression.Constant(1));

            IncDecExpressionVisitor visitor = new IncDecExpressionVisitor(variable);


            Expression transformedIncrement = visitor.Visit(incrementExpression);
            Console.WriteLine("Original Increment: " + incrementExpression);
            Console.WriteLine("Transformed Increment: " + transformedIncrement);

            Expression transformedDecrement = visitor.Visit(decrementExpression);
            Console.WriteLine("Original Decrement: " + decrementExpression);
            Console.WriteLine("Transformed Decrement: " + transformedDecrement);
        }
        static void TestLambdaParameterReplacementWithTwoParams()
        {
            var x = Expression.Parameter(typeof(int), "x");
            var y = Expression.Parameter(typeof(int), "y");
            var sumExpression = Expression.Lambda<Func<int, int, int>>(Expression.Add(x, y), x, y);

            Dictionary<string, object> parameterReplacements = new Dictionary<string, object>
            {
                { "x", 10 },
                { "y", 5 }
            };

            var transformer = new LambdaParameterReplacementVisitor(parameterReplacements);
            var transformedExpression = transformer.Transform<int>(sumExpression);

            Console.WriteLine("Original Expression: " + sumExpression);
            Console.WriteLine("Transformed Expression: " + transformedExpression);
        }
        static void TestLambdaParameterReplacementWithThreeParams()
        {
            // Example usage with three parameters
            var x = Expression.Parameter(typeof(int), "x");
            var y = Expression.Parameter(typeof(int), "y");
            var z = Expression.Parameter(typeof(int), "z");

            // A simple expression with three parameters: x + y + z
            var sumExpression = Expression.Lambda<Func<int, int, int, int>>(Expression.Multiply(Expression.Add(x, y), z), x, y, z);

            Dictionary<string, object> parameterReplacements = new Dictionary<string, object>
                {
                    { "x", 10 },
                    { "y", 5 },
                    { "z", 3 }
                };

            // Transform the expression
            var transformer = new LambdaParameterReplacementVisitor(parameterReplacements);
            var transformedExpression = transformer.Transform<int>(sumExpression);

            // Print the original and transformed expressions
            Console.WriteLine("Original Expression: " + sumExpression);
            Console.WriteLine("Transformed Expression: " + transformedExpression);
        }
    }
}
