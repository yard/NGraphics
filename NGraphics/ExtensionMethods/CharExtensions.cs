namespace NGraphics.Custom.ExtensionMethods
{
    public static class CharExtensions
    {
        public static bool IsAbsolute(this char operation)
        {
            return !char.IsLower(operation);
        }

        public static bool IsNumberSeparator(this char character)
        {
            switch (character)
            {
                case ' ':
                case ',':
                case '\n':
                case '\t':
                    return true;
            }

            return false;
        }
    }
}