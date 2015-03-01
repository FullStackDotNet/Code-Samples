namespace CSharpCodeSamples.Common.Interfaces.Models.Definitions
{
    using System.Collections.Generic;

    public interface ICommandLineData
    {
        Dictionary<string, IEntityTypeBrief>  EntityAliases     { get; }     //alias, (name,flags)
        Dictionary<string, IEntityDefinition> EntityDefinitions { get; }     //entity, definitions
    }
}
