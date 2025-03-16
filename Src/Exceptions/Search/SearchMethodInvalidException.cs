namespace Shorpy.Exceptions;
public class SearchMethodInvalidException : ShorpyException
{
  public SearchMethodInvalidException()
        : base("The provided search method does not support the field type", "SnS_006") { }
}