using NGraphics.Custom.Codes;
using NGraphics.Custom.ExtensionMethods;

namespace NGraphics.Custom.Models
{
    public class Operation
    {
        public char OriginalValue { get; set; }

        public bool IsAbsolute
        {
            get { return OriginalValue.IsAbsolute(); }
        }

        public OperationType Type { get; set; }
    }
}