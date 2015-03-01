namespace CSharpCodeSamples.Definitions
{
    using Common.Interfaces.Models.Definitions;

    internal class EntityTypeBrief : IEntityTypeBrief
    {
        public string AliasName    { get; set; }
        public string Name         { get; set; }
        public string PermName     { get; set; }
        public bool   IsAddable    { get; set; }
        public bool   IsDeletable  { get; set; }
        public bool   IsQueueable  { get; set; }
        public string MenuTemplate { get; set; }
    }
}
