namespace CSharpCodeSamples.Messaging.Requests.Payloads
{
    using Common.Enumerations;
    using Common.Interfaces.Messaging.Requests.Payloads;
    using Common.Interfaces.Models;

    public class Payload_Update : IPayload_Update, IPayload_Update_Build
    {
        public PayloadTypes  PayloadType      { get { return PayloadTypes.Update; } }

        public string        Comments         { get; set; }
        public ISearchItem   ExistingField    { get; set; }
        public ISearchItem   RowKeyValue      { get; set; }
        public IDynamicValue UpdateFieldValue { get; set; }
    }
}
