namespace CSharpCodeSamples.Messaging.Requests
{
    using System.Configuration;

    using Common;
    using Common.Interfaces.Messaging.Requests;
    using Common.Interfaces.Models;
    
    /// <summary>
    /// Represents the unparsed user request.
    /// </summary>
    public class UserRequest : IUserRequest
    {
        protected string _entityTypeName;

        /// <summary>
        /// The command line text, as entered by the user, to be parsed and processed.
        /// </summary>
        public string       CommandLine   { get; private set; }
        /// <summary>
        /// The name of the entity being dealt with (i.e. Locations, Orders, Sales Reps, Customers, whatever you are working with).
        /// For example, if the request is searching orders, then the entity name will be "ORDERS", or its abbreviation ("OR").
        /// The origin of these entity names is in the config file and correlate directly to EF Models.
        /// </summary>
        /// <exception cref="ConfigurationErrorsException" accessor="get">Access to uninitialized user request.  Invalid entity type</exception>
        public string       EntityName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_entityTypeName))
                    throw new ConfigurationErrorsException("Access to uninitialized user request.  Invalid entity type");
                return _entityTypeName;

            }
        }
        /// <summary>
        /// A flag indicating whether the results should be display as a grid.
        /// </summary>
        /// <remarks>
        /// versus a representation that is more responsive.  With an excel like grid (jquery-ui, telerik, infragistics) you can only go so narrow before it gets silly.
        /// </remarks>
        public bool         DisplayAsGrid { get; private set; }
        /// <summary>
        /// The <seealso cref="UserContext"/> object representing the current user and request.
        /// </summary>
        public IUserContext UserContext   { get; private set; }



        public UserRequest()
        {
            UserContext   = null;
            DisplayAsGrid = false;
        }

        /// <summary>
        /// Returns a blank IUserRequest for the given entity type.
        /// </summary>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <returns>A blank UserRequest (as IUserRequest) configured for the provided entity type.</returns>
        public IUserRequest ForEntity(string entityTypeName)
        {
            _entityTypeName = entityTypeName;
            return this;
        }
        /// <summary>
        /// Returns a partially configured IUserRequest with the provided command line.
        /// </summary>
        /// <param name="commandLine">The raw command line text from the user entry.</param>
        /// <returns>A partially configured UserRequest (as IUserRequest) with the provided command line.</returns>
        public IUserRequest UseCommandLine(string commandLine)
        {
            CommandLine = commandLine + Constants.DELIMITER_SPACE;
            return this;
        }
        /// <summary>
        /// Returns the current UserRequest object with a modified 'display as grid' value.
        /// </summary>
        /// <param name="displayAsGrid">if set to <c>true</c> or not specified, sets DisplayAsGrid property to true; otherwise <c>false</c>.</param>
        /// <returns>The UserRequest with the addition of a modified DisplayAsGrid property.</returns>
        public IUserRequest WithGrid(bool displayAsGrid = true)
        {
            DisplayAsGrid = displayAsGrid;
            return this;
        }
        /// <summary>
        /// Returns the current UserRequest object with the addition of the supplied user context.
        /// </summary>
        /// <param name="userContext">A populated <seealso cref="UserContext"/> object.</param>
        /// <returns>The UserRequest with the additional user context configuration.</returns>
        public IUserRequest WithUserContext(IUserContext userContext) {
            UserContext = userContext;
            return this;
        }
    }
}
