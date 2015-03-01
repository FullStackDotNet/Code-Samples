namespace CSharpCodeSamples.Common.Interfaces.Models
{
    using System;
    using System.Linq.Expressions;

    using Enumerations;
    using Definitions;

    /// <summary>
    /// Interface ISearchItem
    /// Represents a search item as a single unit.
    /// contains the field definition of the field being searched (possibly the crossfield placeholder)
    /// the operator specified and the value specified.
    /// Also provides helpers for the dynamic compilation.
    /// </summary>
    public interface ISearchItem //: IDynamicValue
    {
        /// <summary>
        /// The field definition representing the field to be searched.  <seealso cref="IFieldDefinition"/>
        /// </summary>
        IFieldDefinition     Field       { get; }
        /// <summary>
        /// The operator (=, ~, >, etc.) that is associated with the value.
        /// </summary>
        SearchFieldOperators Operator    { get; }
        /// <summary>
        /// The search value itself.  dynamic holding Date, string, numeric
        /// </summary>
        dynamic              SearchValue { get; }

        /// <summary>
        /// Black Magic
        /// </summary>
        Expression<Func<T, bool>> LinqQueryFunction<T>();
    }
}
