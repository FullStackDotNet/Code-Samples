namespace CSharpCodeSamples.Messaging.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Common.Enumerations;
    using Common.Interfaces.Messaging.Requests;
    using Common.Interfaces.Messaging.Requests.Payloads;
    using Common.Interfaces.Messaging.Responses;
    using Common.Interfaces.Models;
    using Common.Interfaces.Models.Definitions;
    using Domain.Models;
    using Responses;

    /// <summary>
    /// Represents the fully parsed user request
    /// </summary>
    public class ParsedRequest : UserRequest, IParsedRequest, IParsedRequest_Build
    {
        private const string FIELDNAME_CROSSFIELD = "";

        private readonly Dictionary<string, List<ISearchItem>> _searchFieldItems;

        /// <summary>
        /// Convenience method, returns list of <seealso cref="ISearchItem"/> that apply to those fields marked as available for cross field search.
        /// </summary>
        public List<ISearchItem>      CrossFieldSearchItems     { get { return WithField(FIELDNAME_CROSSFIELD); } }
        /// <summary>
        /// Returns a list of <seealso cref="IFieldDefinition"/> for those fields marked as available for cross field search.
        /// </summary>
        public List<IFieldDefinition> CrossFieldTargets         { get; private set; }
        /// <summary>
        /// Returns an enumerable of string, each string representing on of the field names that will be referenced by the current parsed search.
        /// </summary>
        public IEnumerable<string>    FieldNamesToSearch
        {
            get
            {
                return _searchFieldItems.Keys.Where(i => i != FIELDNAME_CROSSFIELD);
            }
        }
        /// <summary>
        /// Geta a value indicating whether the parsed request has cross field search items.
        /// </summary>
        /// /// <value><c>true</c> if the parsed request has cross field search items; otherwise, <c>false</c>.</value>
        public bool                   HaveCrossFieldSearchItems { get { return HaveSearchItemsForField(FIELDNAME_CROSSFIELD); } }
        /// <summary>
        /// Gets a value indicating whether the parsed request has one or more payloads.
        /// </summary>
        /// <value><c>true</c> if the parsed request has one or more payloads; otherwise, <c>false</c>.</value>
        public bool                   HavePayload               { get { return Payloads.Any(); } }
        /// <summary>
        /// Returns a list of string where each string represents an error (or warning) which occurred during parse.
        /// </summary>
        /// <value>The list of parse errors.</value>
        public List<string>           ParseErrors               { get; private set; }
        /// <summary>
        /// A list of <seealso cref="IPayload"/> which contains payloads found during the parse.
        /// Note: will be empty (not null) if there were no payloads found.
        /// </summary>
        /// <value>The list of payloads.</value>
        public List<IPayload>         Payloads                  { get; private set; }

        public ParsedRequest()
        {
            ParseErrors       = new List<string>();
            Payloads          = new List<IPayload>();
            _searchFieldItems = new Dictionary<string, List<ISearchItem>>();
        }

        /// <summary>
        /// Accepts a list of <seealso cref="IFieldDefinition"/> and an <seealso cref="IDynamicValue"/>
        /// and adds them to the search field item list.
        /// Operator specified is validated and may be changed if invalid.
        /// </summary>
        /// <param name="fieldList">The list of fields to which the search value applies.</param>
        /// <param name="value">The parsed search value to add.</param>
        public void AddParsedSearchValueToRequest(List<IFieldDefinition> fieldList,
                                                  IDynamicValue          value)
        {
            Debug.Assert(fieldList != null && fieldList.Count > 0);
            Debug.Assert(value != null);

            foreach (IFieldDefinition fd in fieldList)
            {
                AddParsedSearchValueToRequest(fd, value);
            }
        }
        /// <summary>
        /// Accepts a single <seealso cref="IFieldDefinition"/> and an <seealso cref="IDynamicValue"/>
        /// and adds them to the search field item list.
        /// Operator specified is validated and may be changed if invalid.
        /// </summary>
        /// <param name="fd">The field to which the search value applies.</param>
        /// <param name="value">The parsed search value to add.</param>
        internal void AddParsedSearchValueToRequest(IFieldDefinition fd,
                                                    IDynamicValue    value)
        {
            Debug.Assert(fd != null);
            Debug.Assert(value != null);

            string fieldName = fd.Name;
            if (!_searchFieldItems.ContainsKey(fieldName))
                _searchFieldItems[fieldName] = new List<ISearchItem>();

            SearchFieldOperators updatedOperator = ValidateComparisonOperations(fd, value);

            ISearchItem item = new SearchItem(fd, value, updatedOperator);
            if (!_searchFieldItems[fieldName].Any(i => i.Operator == item.Operator &&
                                                       i.SearchValue.ToString() == item.SearchValue.ToString()))
                _searchFieldItems[fieldName].Add(item);
        }
        internal void AddParseErrorAndDisableActions(string parseError)
        {
            ParseErrors.Add(parseError);
        }

        /// <summary>
        /// Adds the payload to the parsed request.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <exception cref="ArgumentNullException">The value of 'payload' cannot be null. </exception>
        public void AddPayload(IPayload payload)
        {
            if (payload == null) throw new ArgumentNullException("payload");
            Payloads.Add(payload);
        }
        /// <summary>
        /// Adds one or more payloads to the parsed request.
        /// </summary>
        /// <param name="payloads">The payloads to add.</param>
        /// <exception cref="ArgumentNullException">The value of 'payloads' cannot be null. </exception>
        public void AddPayloads(IEnumerable<IPayload_Update> payloads)
        {
            if(payloads == null) throw new ArgumentNullException("payloads");

            foreach (IPayload_Update payload in payloads.Where(ValidatePayload))
            {
                Payloads.Add(payload);
            }
        }
        /// <summary>
        /// Clears all search items from the specified field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public void ClearSearchItemsForField(string fieldName)
        {
            if (_searchFieldItems.ContainsKey(fieldName))
            {
                _searchFieldItems[fieldName].Clear();
            }
        }
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
        public IParsedRequest ForEntity(string entityName, List<IFieldDefinition> crossFieldTargets)
        {
            _entityTypeName   = entityName;
            CrossFieldTargets = crossFieldTargets;
            return this;
        }
        /// <summary>
        /// Returns a value indicating whether the parsed request has one or more payloads of the specified payload type..
        /// </summary>
        /// <param name="payloadType">Type of the payload.</param>
        /// <returns><c>true</c> if the parsed request has one or more payloads of type, <c>false</c> otherwise.</returns>
        public bool HavePayloadType(PayloadTypes payloadType)
        {
            return Payloads.Any(p => p.PayloadType == payloadType);
        }
        /// <summary>
        /// Returns a value indicating whether the parsed request has search items for the indicated field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns><c>true</c> if the parsed request has search items for field, <c>false</c> otherwise.</returns>
        public bool HaveSearchItemsForField(string fieldName)
        {
            return (_searchFieldItems.ContainsKey(fieldName) &&
                    _searchFieldItems[fieldName].Any());
        }
        /// <summary>
        /// Returns a list of SearchItems corresponding to the specified field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The list of SearchItems that corresponds to the specified field name.</returns>
        /// <exception cref="ArgumentException">Condition. </exception>
        public List<ISearchItem> WithField(string fieldName)
        {
            if (!HaveSearchItemsForField(fieldName))
                throw new ArgumentException(string.Format("Invalid Field Value: {0}", fieldName));

            // ReSharper disable once ExceptionNotDocumented
            return _searchFieldItems[fieldName];
        }

        public IResponse CreateResponse(IMixedResult[] results)
        {
            return new Response(this, results);
        }

        /// <summary>
        /// This certainly isn't bullet proof and merely provides minimal validation.
        /// In an environment where edge cases needed to be thoroughly dealt with, you'd want to expand this.
        /// </summary>
        private SearchFieldOperators ValidateComparisonOperations(IFieldDefinition fd, IDynamicValue value)
        {
            if (value.Operator != SearchFieldOperators.LessThan &&
                value.Operator != SearchFieldOperators.GreaterThan) return value.Operator;

            ISearchItem existingOperation   = _searchFieldItems[fd.Name].FirstOrDefault(i => i.Operator == value.Operator);
            ISearchItem existingGreaterThan = _searchFieldItems[fd.Name].FirstOrDefault(i => i.Operator == SearchFieldOperators.GreaterThan);
            ISearchItem existingLessThan    = _searchFieldItems[fd.Name].FirstOrDefault(i => i.Operator == SearchFieldOperators.LessThan);
            if (existingOperation != null)
            {
                if (value.SearchValue == existingOperation.SearchValue) return value.Operator;
                AddParseErrorAndDisableActions("Multiple incompatible operators were specified for " +
                                                (fd.Name == FIELDNAME_CROSSFIELD
                                                    ? "cross field search"
                                                    : "field name " + fd.Name) +
                                               ".  Operator was ignored.");
                return SearchFieldOperators.Default;
            }
            if (value.Operator == SearchFieldOperators.LessThan &&
                existingGreaterThan != null)
            {
                if (value.CompareTo(existingGreaterThan) < 0)
                {
                    AddParseErrorAndDisableActions("Greater-Than and Less-Than operations are incompatible for " +
                                                    (fd.Name == ""
                                                        ? "cross field search"
                                                        : "field name " + fd.Name) +
                                                   ".  Operator was ignored.");
                    return SearchFieldOperators.Default;
                }
            }
            if (value.Operator == SearchFieldOperators.GreaterThan &&
                existingLessThan != null)
            {
                if (value.CompareTo(existingLessThan) > 0)
                {
                    AddParseErrorAndDisableActions("Greater-Than and Less-Than operations are incompatible for " +
                                                    (fd.Name == ""
                                                        ? "cross field search"
                                                        : "field name " + fd.Name) +
                                                   ".  Operator was ignored.");
                    return SearchFieldOperators.Default;
                }
            }
            return value.Operator;
        }
        private bool ValidatePayload(IPayload_Update payload)
        {
            Debug.Assert(payload != null);

            if (payload.ExistingField == null ||
                payload.ExistingField.Field == null ||
                payload.UpdateFieldValue == null)
            {
                // ReSharper disable once ThrowingSystemException
                throw new Exception("Invalid payload specified.");
            }

            if (string.IsNullOrWhiteSpace(payload.Comments))
            {
                ParseErrors.Add(@"No comments were specified for order update action.  Default comment: ""Order updated by [user]"" was used instead.");
                ((IPayload_Update_Build)payload).Comments = "Order updated by [user]";
            }

            return true;
        }
    }
}
