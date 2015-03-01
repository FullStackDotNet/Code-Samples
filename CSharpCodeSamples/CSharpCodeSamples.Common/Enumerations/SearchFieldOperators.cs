namespace CSharpCodeSamples.Common.Enumerations
{
    /// <summary>
    /// Used by parser and Linq to specify the type of operator a given search value should use.
    /// </summary>
    public enum SearchFieldOperators
    {
        AnyNotBlank,
        Contains,
        Default,     //StartsWith
        Equal,
        GreaterThan,
        LessThan,
        NotEqual
    }
}
