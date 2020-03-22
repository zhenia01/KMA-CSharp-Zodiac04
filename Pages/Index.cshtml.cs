using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
  }
}