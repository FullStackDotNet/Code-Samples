namespace CSharpCodeSamples.Common.Interfaces.Messaging.Requests
{
    using System.Collections.Generic;

    using Enumerations;
    using Models;
    using Models.Definitions;
    using Responses;
    using Payloads;

    /// <summary>
    /// Interface IParsedRequest
    /// Represents the fully parsed user request
    /// </summary>
    public interface IParsedRequest : IUserRequest
    {
        /// <summary>
        /// Convenience method, returns list of <seealso cref="ISearchItem"/> that apply to those fields marked as available for cross field search.
        /// </summary>
        List<ISearchItem>      CrossFieldSearchItems     { get; }
        /// <summary>
        /// Returns a list of <seealso cref="IFieldDefinition"/> for those fields marked as available for cross field search.
        /// </summary>
        List<IFieldDefinition> CrossFieldTargets         { get; }
        /// <summary>
        /// Returns an enumerable of string, each string representing on of the field names that will be referenced by the current parsed search.
        /// </summary>
        IEnumerable<string>    FieldNamesToSearch        { get; }
        /// <summary>
        /// Gets a value indicating whether the parsed request has cross field search items.
        /// </summary>
        /// <value><c>true</c> if the parsed request has cross field search items; otherwise, <c>false</c>.</value>
        bool                   HaveCrossFieldSearchItems { get; }
        /// <summary>
        /// Gets a value indicating whether the parsed request has one or more payloads.
        /// </summary>
        /// <value><c>true</c> if the parsed request has one or more payloads; otherwise, <c>false</c>.</value>
        bool                   HavePayload               { get; }
        /// <summary>
        /// Returns a list of string where each string represents an error (or warning) which occurred during parse.
        /// </summary>
        /// <value>The list of parse errors.</value>
        List<string>           ParseErrors               { get; }
        /// <summary>
        /// A list of <seealso cref="IPayload"/> which contains payloads found during the parse.
        /// Note: will be empty (not null) if there were no payloads found.
        /// </summary>
        /// <value>The list of payloads.</value>
        List<IPayload>         Payloads                  { get; }

        /// <summary>
        /// Accepts a list of <seealso cref="IFieldDefinition"/> and an <seealso cref="IDynamicValue"/>
        /// and adds them to the search field item list.
        /// Operator specified is validated and may be changed if invalid.
        /// </summary>
        /// <param name="fieldList">The list of fields to which the search value applies.</param>
        /// <param name="value">The parsed search value to add.</param>
        void AddParsedSearchValueToRequest(List<IFieldDefinition> fieldList, IDynamicValue value);
        /// <summary>
        /// Adds the payload to the parsed request.
        /// </summary>
        /// <param name="payload">The payload.</param>
        void AddPayload(IPayload payload);
        /// <summary>
        /// Adds one or more payloads to the parsed request.
        /// </summary>
        /// <param name="payloads">The payloads to add.</param>
        void AddPayloads(IEnumerable<IPayload_Update> payloads);
        /// <summary>
        /// Clears all search items from the specified field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        void ClearSearchItemsForField(string fieldName);
        /// <summary>
        /// Returns a blank ParsedRequest object for the given entity with the specified cross field targets.
        /// </summary>
        /// <param name="entityName">
        /// The entity name which the parsed request is concerned with ParsedRequest.
        /// For example, if the parsed request is searching orders, then the entity name will be "ORDERS".
        /// The origin of these entity names is in the config file and correlate directly to EF Models.
        /// </param>
        /// <param name="crossFieldTargets">
        /// The cross field targets used to create the ParsedRequest.
        /// This is a list of <seealso cref="IFieldDefinition"/> which represent those fields marked as available for cross field search.
        /// </param>
        /// <returns>A blank ParsedRequest (as IParsedRequest).</returns>
        IParsedRequest ForEntity(string entityName, List<IFieldDefinition> crossFieldTargets);
        /// <summary>
        /// Returns a value indicating whether the parsed request has one or more payloads of the specified payload type..
        /// </summary>
        /// <param name="payloadType">Type of the payload.</param>
        /// <returns><c>true</c> if the parsed request has one or more payloads of type, <c>false</c> otherwise.</returns>
        bool HavePayloadType(PayloadTypes payloadType);
        /// <summary>
        /// Returns a value indicating whether the parsed request has search items for the indicated field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns><c>true</c> if the parsed request has search items for field, <c>false</c> otherwise.</returns>
        bool HaveSearchItemsForField(string fieldName);
        /// <summary>
        /// Returns a list of SearchItems corresponding to the specified field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The list of SearchItems that corresponds to the specified field name.</returns>
        List<ISearchItem> WithField(string fieldName);
    }

    public interface IParsedRequest_Build
    {
        IResponse CreateResponse(IMixedResult[] results);
    }
}
