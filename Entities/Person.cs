using System;
using System.Net.Mail;
using System.Threading.Tasks;
using BorodaikevychZodiac.Exceptions;

namespace BorodaikevychZodiac.Entities
{
  public class Person
  {
    public Person(string firstName, string lastName, string email, DateTime birthDate = default)
    {
      FirstName = firstName;
      LastName = lastName;
      Email = email;
      SetBirthDateAsync(birthDate).Wait();
    }

    public Person(string firstName, string lastName, DateTime birthDate) : this(firstName, lastName, "", birthDate)
    {
    }

    public Person(){}

    public string FirstName { get; set; }
    public string LastName { get; set; } 

    private string _email;
    public string Email
    {
      get => _email;
      set
      {
        try
        {
          var email = new MailAddress(value);
          _email = value;
        }
        catch (FormatException)
        {
          throw new InvalidEmailFormatException();
        }
      }
    }

    private readonly BirthInfo _birthInfo = new BirthInfo();

    public DateTime BirthDate => _birthInfo.BirthDate;

    public async Task SetBirthDateAsync(DateTime birthDate)
      {
        await _birthInfo.SetBirthDateAsync(birthDate);
        ChineseZodiacSign = await ZodiacSigns.ChineseSign(_birthInfo.BirthDate);
        WesternZodiacSign = await ZodiacSigns.WesternSign(_birthInfo.BirthDate);
        IsAdult = _birthInfo.Age >= 18;
      }
    

    public bool IsAdult { get; private set; }
    public (string name, string emoji) ChineseZodiacSign { get; private set; }
    public (string name, string emoji) WesternZodiacSign { get; private set; }
    public bool IsBornToday => _birthInfo.IsBornToday;
  }
}