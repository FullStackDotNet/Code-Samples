namespace CSharpCodeSamples.Domain.Models
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    
    using Common.Enumerations;
    using Common.Interfaces.Models;
    using Common.Interfaces.Models.Definitions;
    
    /// <summary>
    /// Represents a search item as a single unit.
    /// contains the field definition of the field being searched (possibly the crossfield placeholder)
    /// the operator specified and the value specified.
    /// Also provides helpers for the dynamic compilation.
    /// </summary>
    public class SearchItem : ISearchItem
    {
        private const string METHODNAME_CONTAINS   = "Contains";
        private const string METHODNAME_STARTSWITH = "StartsWith";
        private const string METHODNAME_TOUPPER    = "ToUpper";

        private readonly static Type       _typeString        = typeof (string);

        private readonly        MethodInfo _method_Contains   = _typeString.GetMethod(METHODNAME_CONTAINS,   new[] { _typeString });
        private readonly        MethodInfo _method_StartsWith = _typeString.GetMethod(METHODNAME_STARTSWITH, new[] { _typeString });
        private readonly        MethodInfo _method_ToUpper    = _typeString.GetMethod(METHODNAME_TOUPPER,    Type.EmptyTypes);

        /// <summary>
        /// The field definition representing the field to be searched.  <seealso cref="IFieldDefinition"/>
        /// </summary>
        public IFieldDefinition     Field            { get; private set; }
        /// <summary>
        /// The operator (=, ~, >, etc.) that is associated with the value.
        /// </summary>
        public SearchFieldOperators Operator         { get; private set; }
        /// <summary>
        /// The search value itself.  dynamic holding Date, string, numeric
        /// </summary>
        public dynamic              SearchValue      { get; private set; }


        /// <summary>
        /// Creates expression representing search item for use in dynamic expression tree
        /// </summary>
        public Expression<Func<T, bool>> LinqQueryFunction<T>()
        {
            Expression expression;
            Expression columnNameProperty = Field.ColumnNameExpression();

            if (Field.DataType == typeof (bool))
            {
                Expression columnValue = Expression.Constant(true, typeof(bool));
                expression = Expression.Equal(columnNameProperty, columnValue);
            }
            else
            {
                expression = Field.DataType == typeof(DateTime)
                                    ? BuildSelectExpressionForDate(columnNameProperty)
                                    : BuildSelectExpressionForStringOrNumeric(Field, columnNameProperty);   
            }

            return expression == null
                       ? null
                       : Expression.Lambda<Func<T, bool>>(expression, new[] { Field.RootEntitySetExpression });
        }

        /// <summary>
        /// Creates expression representing search value select if dynamic value is date.
        /// </summary>
        private Expression BuildSelectExpressionForDate(Expression columnNameProperty)
        {
            DateTime searchValueDate = (DateTime)SearchValue;

            Expression result;
            
            Expression leftExpressionYear   = Expression.Property(columnNameProperty, "Year");
            Expression leftExpressionMonth  = Expression.Property(columnNameProperty, "Month");
            Expression leftExpressionDay    = Expression.Property(columnNameProperty, "Day");
            Expression rightExpressionYear  = Expression.Constant(searchValueDate.Year,  typeof(int));
            Expression rightExpressionMonth = Expression.Constant(searchValueDate.Month, typeof(int));
            Expression rightExpressionDay   = Expression.Constant(searchValueDate.Day,   typeof(int));
            Expression columnValue          = Expression.Constant(SearchValue,           typeof(DateTime));

            Expression expressionYear;
            Expression expressionMonth;
            Expression expressionDay;
            
            switch (Operator)
            {
                case SearchFieldOperators.Default:
                case SearchFieldOperators.Equal:
                    expressionYear  = Expression.Equal(leftExpressionYear,  rightExpressionYear);
                    expressionMonth = Expression.Equal(leftExpressionMonth, rightExpressionMonth);
                    expressionDay   = Expression.Equal(leftExpressionDay,   rightExpressionDay);
                    result = Expression.And(expressionYear, expressionMonth);
                    result = Expression.And(result, expressionDay);
                    break;
                case SearchFieldOperators.GreaterThan:
                    result = Expression.GreaterThanOrEqual(columnNameProperty, columnValue);
                    break;
                case SearchFieldOperators.LessThan:
                    result = Expression.LessThanOrEqual(columnNameProperty, columnValue);
                    break;
                case SearchFieldOperators.NotEqual:
                    expressionYear  = Expression.NotEqual(leftExpressionYear,  rightExpressionYear);
                    expressionMonth = Expression.NotEqual(leftExpressionMonth, rightExpressionMonth);
                    expressionDay   = Expression.NotEqual(leftExpressionDay,   rightExpressionDay);
                    result = Expression.Or(expressionYear, expressionMonth);
                    result = Expression.Or(result, expressionDay);
                    break;
                case SearchFieldOperators.AnyNotBlank:
                case SearchFieldOperators.Contains:
                default:
                    throw new NotImplementedException();
            }
            return result;
        }

        /// <summary>
        /// Creates expression representing search value select if dynamic value is string or numeric.
        /// </summary>
        private Expression BuildSelectExpressionForStringOrNumeric(IFieldDefinition target, Expression columnNameProperty)
        {
            Expression result;
            Expression columnValue = null;
            Expression leftExpression;
            switch (target.DataType.ToString().ToUpperInvariant())
            {
                case "SYSTEM.DECIMAL":
                    if (SearchValue is decimal)
                    {
                        columnValue = Expression.Constant(SearchValue, typeof (decimal));
                    }
                    leftExpression = columnNameProperty;
                    break;
                case "SYSTEM.INT32":
                    if (SearchValue is int)
                    {
                        columnValue = Expression.Constant(SearchValue, typeof (int));
                    }
                    leftExpression = columnNameProperty;
                    break;
                default:
                    columnValue = Expression.Constant(SearchValue, typeof (string));
                    leftExpression = Expression.Call(columnNameProperty, _method_ToUpper);
                    break;
            }
            if (columnValue == null) return null;

            if (target.DataType.ToString().ToUpperInvariant() == "SYSTEM.DECIMAL" ||
                target.DataType.ToString().ToUpperInvariant() == "SYSTEM.INT32")
            {
                switch (Operator)
                {
                    case SearchFieldOperators.AnyNotBlank:
                    case SearchFieldOperators.NotEqual:
                        result = Expression.NotEqual(leftExpression, columnValue);
                        break;
                    case SearchFieldOperators.Equal:
                    case SearchFieldOperators.Default:
                        result = Expression.Equal(leftExpression, columnValue);
                        break;
                    case SearchFieldOperators.GreaterThan:
                        result = Expression.GreaterThanOrEqual(leftExpression, columnValue);
                        break;
                    case SearchFieldOperators.LessThan:
                        result = Expression.LessThanOrEqual(leftExpression, columnValue);
                        break;
                    case SearchFieldOperators.Contains:
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                switch (Operator)
                {
                    case SearchFieldOperators.AnyNotBlank:
                    case SearchFieldOperators.NotEqual:
                        result = Expression.NotEqual(leftExpression, columnValue);
                        break;
                    case SearchFieldOperators.Equal:
                        result = Expression.Equal(leftExpression, columnValue);
                        break;
                    case SearchFieldOperators.Contains:
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        result = Expression.Call(leftExpression, _method_Contains, columnValue);
                        break;
                    case SearchFieldOperators.GreaterThan:
                    case SearchFieldOperators.LessThan:
                    case SearchFieldOperators.Default:
                        // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                        result = Expression.Call(leftExpression, _method_StartsWith, columnValue);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return result;
        }

        public SearchItem(IFieldDefinition field, SearchFieldOperators fieldOperator, DateTime value)
            
        {
            Field            = field;
            Operator         = fieldOperator;
            SearchValue      = value;
        }

        public SearchItem(IFieldDefinition field, SearchFieldOperators fieldOperator, decimal value)
        {
            Field            = field;
            Operator         = fieldOperator;
            SearchValue      = value;
        }

        public SearchItem(IFieldDefinition field, SearchFieldOperators fieldOperator, string value, bool isFirstName = false)
        {
            Field            = field;
            Operator         = fieldOperator;
            SearchValue      = value;
        }

        public SearchItem(IFieldDefinition field, IDynamicValue value, SearchFieldOperators? overrideFieldOperator = null)
        {
            Field            = field;
            Operator         = overrideFieldOperator ?? value.Operator;
            SearchValue      = value.SearchValue;
        }
    }
}
