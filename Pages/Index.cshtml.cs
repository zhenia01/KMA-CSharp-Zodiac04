using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using BorodaikevychZodiac.Exceptions;
using BorodaikevychZodiac.Models.PersonList;
using BorodaikevychZodiac.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BorodaikevychZodiac.Pages
{
  public class IndexModel : PageModel
  {
    private readonly PersonListModel _personList = new PersonListModel();
    public List<UserModel> PersonList => _personList.PersonList;
    public EditPersonPartialModel EditModel { get; } = new EditPersonPartialModel();

    public async Task OnGet()
    {
      await _personList.Initialize();
    }

    public async Task<IActionResult> OnPostEditPersonAsync(string birthDate, string email)
    {
      try
      {
        await EditModel.EditAsync(birthDate, email);
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
      catch (InvalidEmailFormatException e)
      {
        ModelState.AddModelError("Email", e.Message);
      }

      return Partial("_ContactModalPartial", this);
    }
  }
}