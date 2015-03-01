namespace CSharpCodeSamples
{
    using Microsoft.Practices.Unity;

    using Common.Interfaces.Messaging.Requests;
    using Common.Interfaces.Models.Definitions;
    using Common.Interfaces.Parser;
    
    /// <summary>
    /// This is just a class that I put together in place of the proprietary code that would normally call the parser.
    /// You can think of it as test scaffolding or a good example of how not to use Unity.
    /// </summary>
    public static class ServicesForTheRequest
    {
        private static readonly ICommandLineParser _parser;

        static ServicesForTheRequest()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                _parser = container.Resolve<ICommandLineParser>();
            }
        }

        public static IUserRequest BuildTheUserRequest()
        {
            IUserRequest result;
            using (IUnityContainer container = new UnityContainer())
            {
                result = container.Resolve<IUserRequest>();
            }
            return result;
        }
        
        public static IParsedRequest ParseThisRequest(IUserRequest userRequest)
        {
            return _parser.ParseTheUserRequest(userRequest);
        }

        public static ICommandLineData CommandLineDefs()
        {
            return _parser.CommandLineData;
        }
    }
}
