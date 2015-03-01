namespace CSharpCodeSamples.Common.Interfaces.Messaging.Requests
{
    using Models;

    /// <summary>
    /// Interface IUserRequest
    /// Represents the unparsed user request.
    /// </summary>
    public interface IUserRequest
    {
        /// <summary>
        /// The command line text, as entered by the user, to be parsed and processed.
        /// </summary>
        string       CommandLine   { get; }
        /// <summary>
        /// The name of the entity being dealt with (i.e. Locations, Orders, Sales Reps, Customers, whatever you are working with).
        /// For example, if the request is searching orders, then the entity name will be "ORDERS", or its abbreviation ("OR").
        /// The origin of these entity names is in the config file and correlate directly to EF Models.
        /// </summary>
        string       EntityName    { get; }
        /// <summary>
        /// A flag indicating whether the results should be display as a grid.
        /// </summary>
        /// <remarks>
        /// versus a representation that is more responsive.  With an excel like grid (jquery-ui, telerik, infragistics) you can only go so narrow before it gets silly.
        /// </remarks>
        bool         DisplayAsGrid { get; }

        /// <summary>
        /// The <seealso cref="UserContext"/> object representing the current user and request.
        /// </summary>
        /// <value>The user context for the current request.</value>
        IUserContext UserContext   { get; }

        /// <summary>
        /// Returns a blank IUserRequest for the given entity type.
        /// </summary>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <returns>A blank UserRequest (as IUserRequest) configured for the provided entity type.</returns>
        IUserRequest ForEntity(string entityTypeName);
        /// <summary>
        /// Returns a partially configured IUserRequest with the provided command line.
        /// </summary>
        /// <param name="commandLine">The raw command line text from the user entry.</param>
        /// <returns>A partially configured UserRequest (as IUserRequest) with the provided command line.</returns>
        IUserRequest UseCommandLine(string commandLine);
        /// <summary>
        /// Returns the current UserRequest object with a modified 'display as grid' value.
        /// </summary>
        /// <param name="displayAsGrid">if set to <c>true</c> or not specified, sets DisplayAsGrid property to true; otherwise <c>false</c>.</param>
        /// <returns>The UserRequest with the addition of a modified DisplayAsGrid property.</returns>
        IUserRequest WithGrid(bool displayAsGrid = true);
        /// <summary>
        /// Returns the current UserRequest object with the addition of the supplied user context.
        /// </summary>
        /// <param name="userContext">A populated <seealso cref="UserContext"/> object.</param>
        /// <returns>The UserRequest with the additional user context configuration.</returns>
        IUserRequest WithUserContext(IUserContext userContext);
    }
}
