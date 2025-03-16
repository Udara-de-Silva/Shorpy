namespace Shorpy.Exceptions;
public class SortFieldNotFoundException : ShorpyException
{
  public SortFieldNotFoundException() : base(message: "Sort field not found", code: "SnS_013") { }
}