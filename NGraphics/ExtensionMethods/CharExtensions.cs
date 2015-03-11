namespace NGraphics.ExtensionMethods
{
  public static class CharExtensions
  {
    public static bool IsAbsolute(this char operation)
    {
      return !char.IsLower(operation);
    }
  }
}
