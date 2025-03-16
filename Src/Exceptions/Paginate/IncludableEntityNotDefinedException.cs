namespace Shorpy.Exceptions;
public class IncludableEntityNotDefinedException : ShorpyException
{
  public IncludableEntityNotDefinedException() : base(message: "Includable Entity is not defined in the DTO", code: "Inc_001") { }
}