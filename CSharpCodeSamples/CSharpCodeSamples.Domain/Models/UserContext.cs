namespace CSharpCodeSamples.Domain.Models
{
    using Common.Interfaces.Models;
    
    /// <summary>
    /// Simple but easily extensible object to track values specific to the active user.
    /// </summary>
    public class UserContext : IUserContext
    {
        /// <summary>
        /// A string which represents the browser agent information.
        /// </summary>
        public string        BrowserAgent { get; private set; }
        /// <summary>
        /// The url which initiated the active request.
        /// </summary>
        public string        SourceURL    { get; private set; }
        /// <summary>
        /// The name of the user initiating the request.
        /// </summary>
        public string        UserName     { get; private set; }

        /// <summary>
        /// Initializes the values.
        /// </summary>
        /// <param name="browserAgent">The browser agent.</param>
        /// <param name="sourceURL">The source URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>IUserContext.</returns>
        public IUserContext InitializeValues(string browserAgent, string sourceURL, string userName)
        {
            BrowserAgent = browserAgent;
            SourceURL    = sourceURL;
            UserName     = userName;

            return this;
        }
    }
}
