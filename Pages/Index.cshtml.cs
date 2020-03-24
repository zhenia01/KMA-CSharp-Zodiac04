using System.Threading.Tasks;
using BorodaikevychZodiac.Exceptions;
using BorodaikevychZodiac.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BorodaikevychZodiac.Pages
{
  public class IndexModel : PageModel
  {
    public IActionResult OnGetEditPersonModal(int index)
    {
      return Partial("EditPersonModalPartial", (new PersonModel(), index));
    }

    public IActionResult OnGetAddPersonModal()
    {
      return Partial("AddPersonModalPartial", new PersonModel());
    }

    public IActionResult OnGetPersonList()
    {
      return Partial("PersonListPartial");
    }

    public async Task OnGetDeletePersonAsync(int index)
    {
      await PersonListModel.DeletePerson(index);
    }

    private static async Task<PersonModel> NewPersonModelAsync(string birthDate, string email,
      string firstName,
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

      return person;
    }

    public async Task<IActionResult> OnPostAddPersonAsync(string birthDate, string email,
      string firstName,
      string lastName)
    {
      var person = await NewPersonModelAsync(birthDate, email, firstName, lastName);
      
      if (person.ModelState.IsValid)
      {
        await PersonListModel.AddPerson(person);
      }

      return Partial("AddPersonModalPartial", person);
    }

    public async Task<IActionResult> OnPostEditPersonAsync(int index, string birthDate, string email,
      string firstName,
      string lastName)
    {
      var person = await NewPersonModelAsync(birthDate, email, firstName, lastName);
      
      if (person.ModelState.IsValid)
      {
        await PersonListModel.SetPerson(index, person);
      }

      return Partial("EditPersonModalPartial", (person, index));
    }
  }
}