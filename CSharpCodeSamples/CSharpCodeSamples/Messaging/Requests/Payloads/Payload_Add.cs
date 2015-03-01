namespace CSharpCodeSamples.Messaging.Requests.Payloads
{
    using Common.Enumerations;
    using Common.Interfaces.Messaging.Requests.Payloads;
    using Common.Interfaces.Models;

    public class Payload_Add : IPayload_Add
    {
        public PayloadTypes PayloadType { get { return PayloadTypes.Add; } }
        public ISearchItem  RowKeyValue { get; set; }
    }
}
