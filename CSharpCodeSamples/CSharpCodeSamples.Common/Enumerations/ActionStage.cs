namespace CSharpCodeSamples.Common.Enumerations
{
    /// <summary>
    /// Used by Command Line Parser to track parsing of action commands
    /// </summary>
    public enum ActionStage
    {
        NotInAction,
        InferringAction,
        InAction_Add,
        InAction_Delete,
        InAction_Update,
        InAction_Exiting,
        InAction_SkipToEnd
    }
}
