namespace CSharpCodeSamples.Domain.Models
{
    using System;
    using Common.Enumerations;
    using Common.Interfaces.Models;

    public class DynamicValue : IDynamicValue
    {
        /// <summary>
        /// The operator (=, ~, >, etc.) that is associated with the value.
        /// </summary>
        public SearchFieldOperators Operator         { get; private set; }
        /// <summary>
        /// The search value itself.  dynamic holding Date, string, numeric
        /// </summary>
        public dynamic              SearchValue      { get; private set; }


        public DynamicValue(dynamic value, SearchFieldOperators fieldOperator = SearchFieldOperators.Default)
        {
            Operator         = fieldOperator;
            SearchValue      = value;
        }

        public DynamicValue(IDynamicValue fieldValue, SearchFieldOperators? overrideFieldOperator)
        {
            Operator         = overrideFieldOperator ?? fieldValue.Operator;
            SearchValue      = fieldValue;
        }

        public int CompareTo(IDynamicValue other)
        {
            if (ReferenceEquals(other, null)) return 1;
            if ((other.SearchValue is DateTime &&
                 !(SearchValue is DateTime)) ||
                (other.SearchValue is decimal &&
                 !(SearchValue is decimal)) ||
                (other.SearchValue is string &&
                 !(SearchValue is string)))
            {
                throw new Exception("Invalid comparison performed.");
            }

            return SearchValue.CompareTo(other.SearchValue);
        }

        public int CompareTo(ISearchItem other)
        {
            if (ReferenceEquals(other, null)) return 1;
            if ((other.SearchValue is DateTime &&
                 !(SearchValue is DateTime)) ||
                (other.SearchValue is decimal &&
                 !(SearchValue is decimal)) ||
                (other.SearchValue is string &&
                 !(SearchValue is string)))
            {
                throw new Exception("Invalid comparison performed.");
            }

            return SearchValue.CompareTo(other.SearchValue);
        }

    }
}
