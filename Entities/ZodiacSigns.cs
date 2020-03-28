using System;
using System.Threading.Tasks;

namespace BorodaikevychZodiac.Entities
{
  internal static class ZodiacSigns
  {
    public static Task<(string name, string emoji)> ChineseSign(DateTime birth)
    {
      return Task.Run(() =>
      {
        if (birth == DateTime.MinValue) return default;
        return ((birth.Year - 4) % 12) switch
        {
          0 => ("Rat", "🐀"),
          1 => ("Ox", "🐂"),
          2 => ("Tiger", "🐅"),
          3 => ("Rabbit", "🐇"),
          4 => ("Dragon", "🐉"),
          5 => ("Snake", "🐍"),
          6 => ("Horse", "🐎"),
          7 => ("Goat", "🐐"),
          8 => ("Monkey", "🐒"),
          9 => ("Rooster", "🐓"),
          10 => ("Dog", "🐕"),
          11 => ("Pig", "🐖"),
          _ => default
        };
      });
    }

    public static Task<(string name, string emoji)> WesternSign(DateTime birth)
    {
      return Task.Run(() =>
      {
        if (birth == DateTime.MinValue) return default;
        return birth.Month switch
        {
          1 => (birth.Day <= 20 ? ("Capricorn", "♑") : ("Aquarius", "♒")),
          2 => (birth.Day <= 19 ? ("Aquarius", "♒") : ("Pisces", "♓")),
          3 => (birth.Day <= 20 ? ("Pisces", "♓") : ("Aries", "♈")),
          4 => (birth.Day <= 20 ? ("Aries", "♈") : ("Taurus", "♉")),
          5 => (birth.Day <= 21 ? ("Taurus", "♉") : ("Gemini", "♊")),
          6 => (birth.Day <= 22 ? ("Gemini", "♊") : ("Cancer", "♋")),
          7 => (birth.Day <= 22 ? ("Cancer", "♋") : ("Leo", "♌")),
          8 => (birth.Day <= 23 ? ("Leo", "♌") : ("Virgo", "♍")),
          9 => (birth.Day <= 23 ? ("Virgo", "♍") : ("Libra", "♎")),
          10 => (birth.Day <= 23 ? ("Libra", "♎") : ("Scorpius", "♏")),
          11 => (birth.Day <= 22 ? ("Scorpius", "♏") : ("Sagittarius", "♐")),
          12 => (birth.Day <= 21 ? ("Sagittarius", "♐") : ("Capricorn", "♑")),
          _ => default
        };
      });
    }

}
}
