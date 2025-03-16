namespace Shorpy.Exceptions;
public class IncludeFieldEmptyException : ShorpyException
{
  public IncludeFieldEmptyException() : base(message: "Include field empty", code: "Inc_002") { }
}