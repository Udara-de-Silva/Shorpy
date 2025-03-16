namespace Shorpy.Exceptions;
public class SearchMethodNotSupportedException : ShorpyException
{
  public SearchMethodNotSupportedException()
        : base("Search method not supported", "SnS_008") { }
}