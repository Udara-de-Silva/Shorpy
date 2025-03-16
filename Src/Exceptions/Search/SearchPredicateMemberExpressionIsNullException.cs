namespace Shorpy.Exceptions;
public class SearchPredicateMemberExpressionIsNullException : ShorpyException
{
  public SearchPredicateMemberExpressionIsNullException()
        : base("The ME is null", "SnS_011") { }
}