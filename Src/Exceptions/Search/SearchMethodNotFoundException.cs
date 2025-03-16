namespace Shorpy.Exceptions;
public class SearchMethodNotFoundException : ShorpyException
{
  public SearchMethodNotFoundException()
        : base("Search method not found", "SnS_007") { }
}