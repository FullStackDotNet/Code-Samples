namespace CSharpCodeSamples.Common.Interfaces.Models.Definitions
{
    using System;
    using System.Collections.Generic;

    public interface IFieldGroupDefinition
    {
        string AliasName       { get; set; }
        Type   DataType        { get; set; }
        string DataTypeSimple  { get; }
        bool   IsLongText      { get; set; }
        bool   IsRoot          { get; }

        List<IFieldDefinition> FieldDefinitions { get; } 
    }
}
