using System;

namespace BorodaikevychZodiac.Exceptions
{
  internal class TooEarlyBirthDateException : ArgumentOutOfRangeException
  {
    public TooEarlyBirthDateException(string message) : base(message)
    {
    }
    
    public TooEarlyBirthDateException() : base("Birth date can't be from future")
    {
    }
  }
}
