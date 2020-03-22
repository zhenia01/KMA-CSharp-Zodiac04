using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using BorodaikevychZodiac.Exceptions;
using BorodaikevychZodiac.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BorodaikevychZodiac.Pages
{
  public class IndexModel : PageModel
  {
    private readonly UserModel _user = new UserModel();
    public List<UserModel> PersonList { get; } = new List<UserModel>();

    public string BirthDate => _user.BirthDate;

    [Required]
    [BindProperty]
    public string FirstName
    {
      get => _user.FirstName;
      set => _user.FirstName = value;
    }

    [Required]
    [BindProperty]
    public string LastName
    {
      get => _user.LastName;
      set => _user.LastName = value;
    }

    public string Email => _user.Email;

    public bool IsAdult => _user.IsAdult;
    public bool IsBornToday => _user.IsBornToday;
    public (string name, string emoji) ChineseZodiacSign => _user.ChineseZodiacSign;
    public (string name, string emoji) WesternZodiacSign => _user.WesternZodiacSign;

    public bool Tried { get; private set; }

    public async Task OnPost(string birthDate, string email)
    {
      Tried = true;

      try
      {
        await _user.SetBirthDateStringAsync(birthDate);
      }
      catch (TooEarlyBirthDateException e)
      {
        ModelState.AddModelError("Early birth date", e.Message);
      }
      catch (FutureBirthDateException e)
      {
        ModelState.AddModelError("Future birth date", e.Message);
      }
      catch (InvalidDateFormatException e)
      {
        ModelState.AddModelError("Invalid birth date format", e.Message);
      }

      try
      {
        _user.Email = email;
      }
      catch (InvalidEmailFormatException e)
      {
        ModelState.AddModelError("Email", e.Message);
      }
    }

    public async Task OnGet()
    {
      for (int i = 0; i < 50; i++)
      {
        PersonList.Add(new UserModel
        {
          Email = $"MyMail{i}@gmail.com", FirstName = $"{(char) ('A' + i % 26)}{(char) ('a' + i % 26)}",
          LastName = $"{(char) ('Z' - i % 26)}{(char) ('z' - i % 26)}"
        });
        int d = i % 28 + 1;
        int m = i % 12 + 1;
        int y = 1900 + i;
        var sb = new StringBuilder();
        sb.Append(d <= 9 ? $"0{d}-" : $"{d}-");
        sb.Append(m <= 9 ? $"0{m}-" : $"{m}-");
        sb.Append($"{y}");
        await PersonList[i].SetBirthDateStringAsync(sb.ToString());
      }
    }
  }
}