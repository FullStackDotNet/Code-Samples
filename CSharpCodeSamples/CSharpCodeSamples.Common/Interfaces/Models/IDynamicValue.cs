namespace CSharpCodeSamples.Common.Interfaces.Models
{
    using System;

    using Enumerations;

    /// <summary>
    /// Interface IDynamicValue
    /// </summary>
    public interface IDynamicValue : IComparable<IDynamicValue>
    {
        /// <summary>
        /// The operator (=, ~, >, etc.) that is associated with the value.
        /// </summary>
        SearchFieldOperators Operator         { get; }
        /// <summary>
        /// The search value itself.  dynamic holding Date, string, numeric
        /// </summary>
        dynamic              SearchValue      { get; }

        int CompareTo(ISearchItem other);
    }
}
