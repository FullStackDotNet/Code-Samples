namespace CSharpCodeSamples.Common.Interfaces.Models.Definitions
{
    public interface IResizeSetting
    {
        bool   AllowResizing  { get; }
        int    ColumnMinWidth { get; }
        string Key            { get; }
    }
}
