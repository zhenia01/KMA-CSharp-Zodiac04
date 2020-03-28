using System;

namespace BorodaikevychZodiac.Exceptions
{
  internal class InvalidDateFormatException: FormatException
  {
    public InvalidDateFormatException(string message) : base(message)
    {
    }

    public InvalidDateFormatException() : base("Invalid date format")
    {
    }
  }
}
