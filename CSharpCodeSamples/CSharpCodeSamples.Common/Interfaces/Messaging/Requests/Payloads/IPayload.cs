namespace CSharpCodeSamples.Common.Interfaces.Messaging.Requests.Payloads
{
    using Enumerations;

    /// <summary>
    /// Interface IPayload
    /// </summary>
    public interface IPayload
    {
        PayloadTypes PayloadType { get; }
    }
}
