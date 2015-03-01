namespace CSharpCodeSamples.Common.Interfaces.Models.Definitions
{
    using System;
    using System.Linq.Expressions;

    public interface IFieldDefinition
    {
        Type                DataType                     { get; set; }
        string              Name                         { get; set; }
        ParameterExpression RootEntitySetExpression      { get; set; }
        bool                IsCrossField                 { get; set; }
        bool                IsDefaultDate                { get; set; }
        bool                IsUpdatable                  { get; set; }

        Expression ColumnNameExpression();
    }
}
