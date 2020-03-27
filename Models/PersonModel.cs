using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BorodaikevychZodiac.Entities;
using BorodaikevychZodiac.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BorodaikevychZodiac.Models
{
  public class PersonModel
  {
    private readonly Person _person = new Person();

    public ModelStateDictionary ModelState { get; } = new ModelStateDictionary();

    private string _birthDateString;

    public string BirthDateString
    {
      get => _birthDateString;
      set => SetBirthDateStringAsync(value).Wait();
    }

    public DateTime BirthDateDateTime => _person.BirthDate;

    public async Task SetBirthDateStringAsync(string birthDateString)
    {
      var birthDate = await ParseBirthDateAsync(birthDateString);
      if (birthDate != default)
      {
        _birthDateString = birthDateString;
        await _person.SetBirthDateAsync(birthDate);
      }
      else
      {
        throw new InvalidDateFormatException();
      }
    }

    private static Task<DateTime> ParseBirthDateAsync(string birthDate)
    {
      return Task.Run(() =>
      {
        DateTime.TryParseExact(birthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture,
          DateTimeStyles.None, out var result);
        return result;
      });
    }

    public string FirstName
    {
      get => _person.FirstName;
      set => _person.FirstName = value;
    }

    public string LastName
    {
      get => _person.LastName;
      set => _person.LastName = value;
    }

    public string Email
    {
      get => _person.Email;
      set => _person.Email = value;
    }

    [JsonIgnore] public bool IsBornToday => _person.IsBornToday;
    [JsonIgnore] public bool IsAdult => _person.IsAdult;
    [JsonIgnore] public (string name, string emoji) ChineseZodiacSign => _person.ChineseZodiacSign;
    [JsonIgnore] public (string name, string emoji) WesternZodiacSign => _person.WesternZodiacSign;
  }
}