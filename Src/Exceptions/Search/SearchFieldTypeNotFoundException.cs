namespace Shorpy.Exceptions;
public class SearchFieldTypeNotFoundException : ShorpyException
{
  public SearchFieldTypeNotFoundException()
        : base("Search field type not found", "SnS_004") { }
}
