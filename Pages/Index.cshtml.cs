using System.Collections.Generic;
using System.Threading.Tasks;
using BorodaikevychZodiac.Exceptions;
using BorodaikevychZodiac.Models;
using BorodaikevychZodiac.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BorodaikevychZodiac.Pages
{
  public class IndexModel : PageModel
  {
    private readonly PersonListService _personListService;

    public List<PersonModel> PersonList => _personListService.PersonList;

    public IndexModel(PersonListService personListService)
    {
      _personListService = personListService;
    }

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
      return Partial("PersonListPartial", PersonList);
    }

    public IActionResult OnGetSort(int index)
    {
      if (index >= 0 || index <= 7)
      {
        _personListService.Sort((PersonListActionOption) index);
      }

      return Partial("PersonListPartial", PersonList);
    }

    public IActionResult OnGetApplyFilter(int index, string value)
    {
      if (index >= 0 || index <= 7)
      {
        return Partial("PersonListPartial", _personListService.ApplyFilter((PersonListActionOption)index, value));
      }
      return Partial("PersonListPartial", PersonList);
    }

    public IActionResult OnGetRemoveFilter(int index)
    {
      if (index >= 0 || index <= 7)
      {
        return Partial("PersonListPartial", _personListService.RemoveFilter((PersonListActionOption)index));
      }
      return Partial("PersonListPartial", PersonList);
    }
    
    public IActionResult OnGetRemoveAllFilters()
    {
      _personListService.RemoveAllFilters();
      return OnGetPersonList();
    }

    public async Task OnGetDeletePersonAsync(int index)
    {
      await _personListService.DeletePerson(index);
    }

    private static async Task<PersonModel> NewPersonModelAsync(string birthDateString, string email,
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
        await person.SetBirthDateStringAsync(birthDateString);
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

    public async Task<IActionResult> OnPostAddPersonAsync(string birthDateString, string email,
      string firstName,
      string lastName)
    {
      var person = await NewPersonModelAsync(birthDateString, email, firstName, lastName);
      
      if (person.ModelState.IsValid)
      {
        await _personListService.AddPerson(person);
      }

      return Partial("AddPersonModalPartial", person);
    }

    public async Task<IActionResult> OnPostEditPersonAsync(int index, string birthDateString, string email,
      string firstName,
      string lastName)
    {
      var person = await NewPersonModelAsync(birthDateString, email, firstName, lastName);
      
      if (person.ModelState.IsValid)
      {
        await _personListService.SetPerson(index, person);
      }

      return Partial("EditPersonModalPartial", (person, index));
    }
  }
}