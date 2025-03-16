namespace Shorpy.Exceptions;
public class SearchParameterNotSetException : ShorpyException
{
  public SearchParameterNotSetException()
        : base("The search parameter is not set", "SnS_009") { }
}