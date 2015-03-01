namespace CSharpCodeSamples.Common.Interfaces.Models.Definitions
{
    public interface IEntityTypeBrief
    {
        string AliasName    { get; set; }
        string Name         { get; set; }
        string PermName     { get; set; }
        bool   IsAddable    { get; set; }
        bool   IsDeletable  { get; set; }
        string MenuTemplate { get; set; }
    }
}
