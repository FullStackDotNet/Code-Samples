namespace CSharpCodeSamples.Common.Interfaces.Models
{
    /// <summary>
    /// Interface IUserContext
    /// Simple but easily extensible object to track values specific to the active user.
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// A string which represents the browser agent information.
        /// </summary>
        string BrowserAgent { get; }
        /// <summary>
        /// The url which initiated the active request.
        /// </summary>
        string SourceURL { get; }
        /// <summary>
        /// The name of the user initiating the request.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Initializes the values.
        /// </summary>
        /// <param name="browserAgent">The browser agent.</param>
        /// <param name="sourceURL">The source URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>IUserContext.</returns>
        IUserContext InitializeValues(string browserAgent, string sourceURL, string userName);
    }
}
