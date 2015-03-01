namespace CSharpCodeSamples.Common
{
    using System.Diagnostics;

    public static class Constants
    {
        public static readonly TraceSwitch SystemTraceLevel = new TraceSwitch("TraceLevel", "Level of error to record");

        // Parsing Delimiters
        public const char DELIMITER_CLOSEACTION = ')';
        public const char DELIMITER_FIELDNAME   = ':';
        public const char DELIMITER_OPENACTION  = '(';
        public const char DELIMITER_QUOTE       = '\'';
        public const char DELIMITER_SPACE       = ' ';
    }
}
