using System;

namespace BorodaikevychZodiac.Exceptions
{
  internal class InvalidEmailFormatException : FormatException
  {
    public InvalidEmailFormatException(string message) : base(message)
    {
    }
    
    public InvalidEmailFormatException() : base("Invalid email format")
    {
    }
  }
}