using Expressions.Task3.E3SQueryProvider.Helpers;
using Expressions.Task3.E3SQueryProvider.Models.Request;
using Expressions.Task3.E3SQueryProvider.QueryProvider;
using Expressions.Task3.E3SQueryProvider.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Expressions.Task3.E3SQueryProvider.SqlQueryProvider
{
    public class E3SLinqSqlProvider : IQueryProvider
    {
        private readonly string _connectionString;
        private readonly List<Expression> _expressions;

        public E3SLinqSqlProvider(string connectionString)
        {
            _connectionString = connectionString;
            _expressions = new List<Expression>();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            //Type itemType = TypeHelper.GetElementType(expression.Type);
            Type itemType = GetClassType(expression.Type);

            var translator = new ExpressionToSqlTranslator(itemType);
            string selectQuery = translator.Translate(expression);

            return ExecuteSqlQuery<TResult>(itemType, selectQuery);
        }
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new E3SSqlQuery<TElement>(expression, this);
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        #region Helper
        private Type GetClassType(Type type)
        {
            string typeName = type.GetGenericArguments()[0].FullName;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
        private TResult ExecuteSqlQuery<TResult>(Type itemType, string selectQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var resultList = Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
                        while (reader.Read())
                        {
                            var item = Activator.CreateInstance(itemType);

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                var property = itemType.GetProperty(columnName);

                                if (property != null)
                                {
                                    object value = reader.GetValue(i);
                                    property.SetValue(item, value);
                                }
                            }

                            // Assuming TResult is IEnumerable<T>
                            resultList.GetType().GetMethod("Add").Invoke(resultList, new[] { item });
                        }

                        return (TResult)resultList;
                    }
                }
            }
        }
        #endregion
    }
}
