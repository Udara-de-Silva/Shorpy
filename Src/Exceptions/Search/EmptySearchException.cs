namespace Shorpy.Exceptions;
public class EmptySearchException : ShorpyException
{
  public EmptySearchException()
        : base(message: "The search array is empty", code: "SnS_002") { }
}