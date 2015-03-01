namespace CSharpCodeSamples
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public static class ServicesForParameterExpressions
    {
        private static readonly Dictionary<Type, ParameterExpression> _parameterExpressionCache;

        static ServicesForParameterExpressions()
        {
            _parameterExpressionCache = new Dictionary<Type, ParameterExpression>();
        }

        public static ParameterExpression  GetParamExpressionForEntityType(Type entityType)
        {
            if (!_parameterExpressionCache.ContainsKey(entityType))
            {
                _parameterExpressionCache[entityType] = Expression.Parameter(entityType, entityType.Name);    
            }
            return _parameterExpressionCache[entityType];
        }
    }
}
