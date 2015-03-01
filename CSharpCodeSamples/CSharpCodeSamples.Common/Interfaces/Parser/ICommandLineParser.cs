namespace CSharpCodeSamples.Common.Interfaces.Parser
{
    using Messaging.Requests;
    using Models.Definitions;

    /// <summary>
    /// Interface ICommandLineParser
    /// </summary>
    public interface ICommandLineParser
    {
        ICommandLineData CommandLineData { get; }
        
        IParsedRequest   ParseTheUserRequest(IUserRequest userRequest);
    }
}
