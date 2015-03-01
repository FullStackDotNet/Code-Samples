namespace CSharpCodeSamples.Common.Interfaces.Messaging.Requests.Payloads
{
    using Models;

    /// <summary>
    /// Interface IPayload_Update
    /// </summary>
    public interface IPayload_Update : IPayload
    {
        string        Comments         { get; }
        ISearchItem   ExistingField    { get; }
        ISearchItem   RowKeyValue      { get; set; }
        IDynamicValue UpdateFieldValue { get; }
    }
}
