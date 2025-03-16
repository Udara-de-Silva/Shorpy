namespace Shorpy.Exceptions;
public class EmptySortException : ShorpyException
{
  public EmptySortException()
        : base(message: "The sort array is empty", code: "SnS_012") { }
}
