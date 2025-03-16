/*
File-Name: ShorpyException
File-Description: Base exception for all Shorpy related exceptions
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2022-08-11
Version: 1.0
*/
namespace Shorpy.Exceptions;
public class ShorpyException : Exception
{
  // used to maintain the error code; the error message is taken from the base class Exception
  public readonly string code;
  public ShorpyException(string message, string code)
          : base(message)
  {
    this.code = code;
  }
}