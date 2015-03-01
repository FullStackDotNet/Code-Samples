namespace CSharpCodeSamples.Common.Interfaces.Messaging.Requests.Payloads
{
    using Models;

    /// <summary>
    /// Interface IPayload_Add
    /// </summary>
    public interface IPayload_Add : IPayload
    {
        ISearchItem RowKeyValue { get; set; }
    }
}
