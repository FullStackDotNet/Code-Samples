namespace CSharpCodeSamples.Common.Interfaces.Messaging.Requests.Payloads
{
    using Models;

    /// <summary>
    /// Action Update Stage, Used by parser to assist in tracking payload parse stage
    /// </summary>
    /// <remarks>
    /// Placed here because it is only used in conjunction with the IPayload_Update_Build interface.
    /// </remarks>
    public enum ActionUpdateStage
    {
        None,
        SeekingToValue,
        FoundTOToken,
        FoundTOValue,
        SeekingComments,
        AlmostDone
    }

    /// <summary>
    /// Interface IPayload_Update_Build
    /// </summary>
    public interface IPayload_Update_Build : IPayload
    {
        string        Comments         { set; }
        ISearchItem   ExistingField    { set; }
        IDynamicValue UpdateFieldValue { set; }
    }
}
