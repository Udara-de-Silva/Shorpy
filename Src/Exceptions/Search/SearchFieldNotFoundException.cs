namespace Shorpy.Exceptions;
public class SearchFieldNotFoundException : ShorpyException
{
  public SearchFieldNotFoundException()
        : base("Search field not found", "SnS_003") { }
}
