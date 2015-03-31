using NGraphics.Codes;
using NGraphics.ExtensionMethods;

namespace NGraphics.Models
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