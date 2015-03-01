namespace CSharpCodeSamples.Common.Interfaces.Messaging.Responses
{
    using System.Collections.Generic;

    using Models;

    /// <summary>
    /// Interface IResponse
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Gets the name of the entity which the response corresponds to.
        /// </summary>
        /// <value>The name of the entity (i.e. Events, Orders, Sales Reps).</value>
        string         EntityName     { get; }
        //* Results
        /// <summary>
        /// Gets or sets a value indicating whether actions (if any) attached to the request were processed successfully.
        /// </summary>
        /// <value><c>true</c> if action(s) processed successfully; otherwise, <c>false</c>.</value>
        bool           ActionSuccess  { get; set; }
        /// <summary>
        /// Gets or sets the list of parse errors which occurred.
        /// </summary>
        /// <value>The list of parse errors.</value>
        List<string>   ParseErrors    { get; set; }
        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>The results.</value>
        IMixedResult[] Results        { get; set; }
        /// <summary>
        /// Gets the total number of items available as results.
        /// </summary>
        /// <value>The total items.</value>
        int            TotalItems     { get; }
    }
}
