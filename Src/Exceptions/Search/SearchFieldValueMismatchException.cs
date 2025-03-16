namespace Shorpy.Exceptions;
public class SearchFieldValueMismatchException : ShorpyException
{
  public SearchFieldValueMismatchException()
        : base("Search value can't be converted to the original search field type", "SnS_005") { }
}
