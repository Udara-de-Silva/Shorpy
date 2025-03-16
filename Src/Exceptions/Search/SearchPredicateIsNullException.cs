namespace Shorpy.Exceptions;
public class SearchPredicateIsNullException : ShorpyException
{
  public SearchPredicateIsNullException()
        : base("The Search Predicate is null", "SnS_010") { }
}