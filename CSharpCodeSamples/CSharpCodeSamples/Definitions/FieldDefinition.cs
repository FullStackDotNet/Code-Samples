namespace CSharpCodeSamples.Definitions
{
    using System;
    using System.Linq.Expressions;

    using Common.Interfaces.Models.Definitions;

    [Serializable]
    internal class FieldDefinition : IFieldDefinition
    {
        public Type                DataType                { get; set; }
        public string              DataTypeSimple          { get { return DataType.ToString().Replace("System.", ""); } }
        public string              Name                    { get; set; }
        
        public Type                EntityType              { get; set; }
        public bool                IsCrossField            { get; set; }
        public bool                IsDefaultDate           { get; set; }
        public bool                IsUpdatable             { get; set; }
        public ParameterExpression RootEntitySetExpression { get; set; }

        public Expression ColumnNameExpression()
        {
            return Expression.Property(RootEntitySetExpression, Name);
        }
    }
}
