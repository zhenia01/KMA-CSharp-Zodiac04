using System.Threading.Tasks;
using BorodaikevychZodiac.Exceptions;
using BorodaikevychZodiac.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BorodaikevychZodiac.Pages
{
  public class IndexModel : PageModel
  {
    public PersonListModel PersonList { get; } = new PersonListModel();

    public async Task<IActionResult> OnGetAsync()
    {
      await PersonList.Initialize();
      return Page();
    }

    public IActionResult OnGetEditPersonModalPartial()
    {
      return Partial("EditPersonModalPartial", new PersonModel());
    }

    public async Task<IActionResult> OnPostEditPersonModalPartialAsync(int index, string birthDate, string email, string firstName,
      string lastName)
    {
      var person = new PersonModel();

      if (string.IsNullOrWhiteSpace(firstName))
      {
        person.ModelState.AddModelError("FirstName", "First Name can't be empty");
      }

      person.FirstName = firstName;

      if (string.IsNullOrWhiteSpace(lastName))
      {
        person.ModelState.AddModelError("LastName", "Last Name can't be empty");
      }

      person.LastName = lastName;

      try
      {
        person.Email = email;
      }
      catch (InvalidEmailFormatException e)
      {
        person.ModelState.AddModelError("Email", e.Message);
      }

      try
      {
        await person.SetBirthDateStringAsync(birthDate);
      }
      catch (TooEarlyBirthDateException e)
      {
        person.ModelState.AddModelError("Early birth date", e.Message);
      }
      catch (FutureBirthDateException e)
      {
        person.ModelState.AddModelError("Future birth date", e.Message);
      }
      catch (InvalidDateFormatException e)
      {
        person.ModelState.AddModelError("Invalid birth date format", e.Message);
      }

      return Partial("EditPersonModalPartial", person);
    }
  }
}