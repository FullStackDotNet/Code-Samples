namespace CSharpCodeSamples.Messaging.Requests.Payloads
{
    using Common.Enumerations;
    using Common.Interfaces.Messaging.Requests.Payloads;

    public class Payload_Delete : IPayload
    {
        public PayloadTypes PayloadType { get { return PayloadTypes.Delete; } }
    }
}
