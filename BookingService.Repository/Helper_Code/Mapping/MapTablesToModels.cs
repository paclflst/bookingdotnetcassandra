using Cassandra.Mapping;
using Cassandra.Mapping.Attributes;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BookingService.Repository.Helper_Code.Mapping
{
    public static class MapTablesToModels
    {
        public static void MapTypes(Type[] types)
        {
            foreach(Type type in types)
            {
                var tyepAttributes = type.GetCustomAttributes(typeof(TableAttribute), true);
                if (tyepAttributes.Any())
                {
                    var tableAttribute = (TableAttribute)tyepAttributes.FirstOrDefault();
                    if (tableAttribute != null)
                    {
                        // Map Table name
                        var mapType = typeof(Map<>).MakeGenericType(type);
                        dynamic map = Activator.CreateInstance(mapType);
                        var mapping = map.TableName(tableAttribute.Name.ToString());

                        foreach (var property in type.GetProperties())
                        {
                            var attributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);
                            if (attributes.Any())
                            {
                                
                                var columnAttribute = (ColumnAttribute)attributes.FirstOrDefault();

                                // Create property expression for column name mapping
                                ParameterExpression parameter = Expression.Parameter(type, string.Empty);
                                MemberExpression field = Expression.PropertyOrField(parameter, property.Name);
                                var delegateType = typeof(Func<,>).MakeGenericType(type, property.PropertyType);
                                LambdaExpression yourExpression = Expression.Lambda(delegateType, field, parameter);

                                // Convert column expression from LambdaExpression to Expression<Func<TEntity,TColumn>>
                                var expressionType = typeof(Expression<>).MakeGenericType(delegateType);
                                dynamic convertedColumn = Convert.ChangeType(yourExpression, expressionType);

                                // Create WithName action for column name mapping
                                var action = new Action<ColumnMap>(cm => cm.WithName(columnAttribute.Name));

                                mapping = mapping.Column(column: convertedColumn, columnConfig: action);
                            }
                        }
                        MappingConfiguration.Global.Define(mapping);
                    }
                }

            }
        }
    }
}
