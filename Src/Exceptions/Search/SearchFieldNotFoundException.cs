namespace Shorpy.Exceptions;
public class SearchFieldNotFoundException : ShorpyException
{
  public SearchFieldNotFoundException()
        : base("Search field not foudnd", "SnS_003") { }
}
