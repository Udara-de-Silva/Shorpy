namespace Shorpy.Exceptions;

public class IncludeFieldNotFoundException : ShorpyException
{
  public IncludeFieldNotFoundException() : base(message: "Include field not found", code: "Inc_003") { }
}