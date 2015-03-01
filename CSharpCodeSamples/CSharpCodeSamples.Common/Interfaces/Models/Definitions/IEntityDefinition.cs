namespace CSharpCodeSamples.Common.Interfaces.Models.Definitions
{
    using System.Collections.Generic;

    public interface IEntityDefinition
    {
        string                                    EntityName           { get; }
        List<IFieldDefinition>                    CrossFieldTargets    { get; }
        IFieldDefinition                          DefaultDate          { get; }
        List<IFieldDefinition>                    DefaultInts          { get; }
        List<IFieldDefinition>                    DefaultDecimals      { get; }
        IEnumerable<IDisplayColumn>               DisplayColumns       { get; }
        SortedList<int, IDisplayColumn>           DisplayColumnsSorted { get; }
        Dictionary<string, IFieldGroupDefinition> FieldGroups          { get; }
        IEnumerable<IResizeSetting>               ResizeSettings       { get; } 

        bool   HasAnyFieldWithDefaultDate { get; }
        bool   HasAnyFieldThatIsDecimal   { get; }
        bool   HasAnyFieldThatIsInt       { get; }
        bool   IsAddable                  { get; }
        bool   IsDeletable                { get; }
        string MenuTemplate               { get; }
        string PermName                   { get; }
        
        IFieldGroupDefinition CrossFieldDefinition();
        bool DoesEntityHaveFieldAlias(string fieldAliasName);
        IFieldGroupDefinition WithSearchField(string fieldAliasName);
    }
}
