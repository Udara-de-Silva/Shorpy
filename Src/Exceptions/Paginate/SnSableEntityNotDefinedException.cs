namespace Shorpy.Exceptions;
public class SnSableEntityNotDefinedException : ShorpyException
{
  public SnSableEntityNotDefinedException() : base(message: "SnSAble Entity is not defined in the DTO", code: "SnS_001") { }
}