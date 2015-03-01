namespace CSharpCodeSamples.Common.Interfaces.Models.Definitions
{
    public interface IDisplayColumn
    {
        string ColumnMinWidth  { get; }
        string ColumnWidth     { get; }
        string DataType        { get; }
        string Key             { get; }
        string HeaderText      { get; }
        string Template        { get; }
        string TextAlign       { get; }
        string TextAlignHeader { get; }
        
        bool   IsHidden        { get; }
        bool   IsUpdatable     { get; }
    }

    public interface IDisplayColumn_Build
    {
        string ColumnMinWidth  { set; }
        string ColumnWidth     { set; }
        string DataType        { set; }
        string Key             { set; }
        string HeaderText      { set; }
        string Template        { set; }
        string TextAlign       { set; }
        string TextAlignHeader { set; }

        bool IsHidden          { set; }
        bool IsUpdatable       { set; }
    }
}
