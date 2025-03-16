namespace Shorpy.Exceptions;
public class SortParameterNotSetException : ShorpyException
{
  public SortParameterNotSetException()
        : base("The sort parameter is not set", "SnS_014") { }
}