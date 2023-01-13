using System;

namespace Vain.Log
{
    [Flags]
    public enum OutputFormat{
        Default = 0,
        Bold = 1,
        Italics = 2,

        Red = 4,
        Green = 8,
        Blue = 16,
        Grey = 32,

    }
    public partial class FormatMasks
    {
        public const OutputFormat Everything = (OutputFormat)63;
        public const OutputFormat ColorMask = OutputFormat.Red | OutputFormat.Green | OutputFormat.Blue| OutputFormat.Grey; 
    }
    
}
