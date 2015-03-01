namespace CSharpCodeSamples.Common.Interfaces.Models.Definitions
{
    public interface ICommandLineDefinitions
    {
        bool                  DoesEntityTypeAliasExist(string entityAlias);
        bool                  DoesEntityTypeExist(string entityTypeName);
        IEntityDefinition     ForEntityType(string entityTypeName);
        bool                  TryParseEntityTypeName(string entityAbbr, out string entityTypeName);
    }
}
