using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BorodaikevychZodiac.Entities;
using BorodaikevychZodiac.Models;

namespace BorodaikevychZodiac.Services
{
  public enum PersonListActionOption // saving\filtering
  {
    FirstName,
    LastName,
    BirthDate,
    Email,
    Adult,
    BirthDay,
    ChineseSign,
    WesternSign,
  }

  public class PersonListService
  {
    public List<PersonModel> PersonList { get; private set; }

    private readonly string _path =
      Path.Combine(Directory.GetCurrentDirectory(), "Storage", "storage.json");

    private readonly BitArray _sortingFlags = new BitArray(8);
    private readonly BitArray _filteringFlags = new BitArray(8);
    private readonly string[] _filteringValues = new string[8];

    public PersonListService()
    {
      try
      {
        using var fs = File.OpenRead(_path);
        PersonList = JsonSerializer.Deserialize<List<PersonModel>>(new StreamReader(fs).ReadToEnd());
      }
      catch (Exception)
      {
        PersonList = new List<PersonModel>();

        string[] firstNames =
        {
          "Adam", "Alex", "Aaron", "Ben", "Carl", "Dan", "David", "Edward", "Fred", "Frank", "George", "Hal", "Hank",
          "Ike", "John", "Jack", "Joe", "Larry", "Monte", "Matthew", "Mark", "Nathan", "Otto", "Paul", "Peter", "Roger",
          "Roger", "Steve", "Thomas", "Tim", "Ty", "Victor", "Walter"
        };

        string[] lastNames =
        {
          "Anderson", "Ashwoon", "Aikin", "Bateman", "Bongard", "Bowers", "Boyd", "Cannon", "Cast", "Deitz", "Dewalt",
          "Ebner", "Frick", "Hancock", "Haworth", "Hesch", "Hoffman", "Kassing", "Knutson", "Lawless", "Lawicki",
          "Mccord", "McCormack", "Miller", "Myers", "Nugent", "Ortiz", "Orwig", "Ory", "Paiser", "Pak", "Pettigrew",
          "Quinn", "Quizoz", "Ramachandran", "Resnick", "Sagar", "Schickowski", "Schiebel", "Sellon", "Severson",
          "Shaffer", "Solberg", "Soloman", "Sonderling", "Soukup", "Soulis", "Stahl", "Sweeney", "Tandy", "Trebil",
          "Trusela", "Trussel", "Turco", "Uddin", "Uflan", "Ulrich", "Upson", "Vader", "Vail", "Valente", "Van Zandt",
          "Vanderpoel", "Ventotla", "Vogal", "Wagle", "Wagner", "Wakefield", "Weinstein", "Weiss", "Woo", "Yang",
          "Yates", "Yocum", "Zeaser", "Zeller", "Ziegler", "Bauer", "Baxster", "Casal", "Cataldi", "Caswell", "Celedon",
          "Chambers", "Chapman", "Christensen", "Darnell", "Davidson", "Davis", "DeLorenzo", "Dinkins", "Doran",
          "Dugelman", "Dugan", "Duffman", "Eastman", "Ferro", "Ferry", "Fletcher", "Fietzer", "Hylan", "Hydinger",
          "Illingsworth", "Ingram", "Irwin", "Jagtap", "Jenson", "Johnson", "Johnsen", "Jones", "Jurgenson", "Kalleg",
          "Kaskel", "Keller", "Leisinger", "LePage", "Lewis", "Linde", "Lulloff", "Maki", "Martin", "McGinnis", "Mills",
          "Moody", "Moore", "Napier", "Nelson", "Norquist", "Nuttle", "Olson", "Ostrander", "Reamer", "Reardon",
          "Reyes", "Rice", "Ripka", "Roberts", "Rogers", "Root", "Sandstrom", "Sawyer", "Schlicht", "Schmitt",
          "Schwager", "Schutz", "Schuster", "Tapia", "Thompson", "Tiernan", "Tisler"
        };

        Random random = new Random();

        for (int i = 0; i < 50; i++)
        {
          var firstName = firstNames[random.Next(0, firstNames.Length)];
          var lastName = lastNames[random.Next(0, lastNames.Length)];
          PersonList.Add(new PersonModel
          {
            FirstName = firstName,
            LastName = lastName,
            Email = $"{firstName}_{lastName}{i}@gmail.com"
          });

          int d = random.Next(1, 28);
          int m = random.Next(1, 12);
          int y = random.Next(1990, 2010);

          var sb = new StringBuilder();
          sb.Append(d <= 9 ? $"0{d}-" : $"{d}-");
          sb.Append(m <= 9 ? $"0{m}-" : $"{m}-");
          sb.Append($"{y}");

          PersonList[i].SetBirthDateStringAsync(sb.ToString()).Wait();
        }

        SerializeList().Wait();
      }
    }

    public async Task<PersonModel> GetPerson(int index)
    {
      PersonList = await DeserializeList();
      return PersonList[index];
    }

    public async Task SetPerson(int index, PersonModel person)
    {
      PersonList[index] = person;
      await SerializeList();
    }

    public async Task DeletePerson(int index)
    {
      PersonList.RemoveAt(index);
      await SerializeList();
    }

    public async Task AddPerson(PersonModel person)
    {
      PersonList.Add(person);
      await SerializeList();
    }

    public void Sort(PersonListActionOption option)
    {
      int index = (int) option;
      _sortingFlags[index] = !_sortingFlags[index];
      PersonList.Sort((p1, p2) =>
      {
        int result = option switch
        {
          PersonListActionOption.FirstName => p1.FirstName.CompareTo(p2.FirstName),
          PersonListActionOption.LastName => p1.LastName.CompareTo(p2.LastName),
          PersonListActionOption.BirthDate => p1.BirthDateDateTime.CompareTo(p2.BirthDateDateTime),
          PersonListActionOption.Email => p1.Email.CompareTo(p2.Email),
          PersonListActionOption.Adult => p1.IsAdult.CompareTo(p2.IsAdult),
          PersonListActionOption.BirthDay => p1.IsBornToday.CompareTo(p2.IsBornToday),
          PersonListActionOption.ChineseSign => p1.ChineseZodiacSign.name.CompareTo(p2.ChineseZodiacSign.name),
          PersonListActionOption.WesternSign => p1.WesternZodiacSign.name.CompareTo(p2.WesternZodiacSign.name),
          _ => -1
        };
        return _sortingFlags[index] ? result : -result;
      });
    }

    public List<PersonModel> ApplyFilter(PersonListActionOption option, string value)
    {
      int index = (int) option;
      _filteringFlags[index] = true;
      _filteringValues[index] = value;
      return Filter();
    }

    public List<PersonModel> RemoveFilter(PersonListActionOption option)
    {
      int index = (int) option;
      _filteringFlags[index] = false;
      _filteringValues[index] = null;
      return Filter();
    }

    public List<PersonModel> RemoveAllFilters()
    {
      for (int i = 0; i < _filteringValues.Length; i++)
      {
        _filteringFlags[i] = false;
        _filteringValues[i] = null;
      }

      return PersonList;
    }

    private List<PersonModel> Filter()
    {
      return PersonList.AsParallel().Where(p =>
        (!_filteringFlags[0] ||
         p.FirstName.Contains(_filteringValues[0], StringComparison.InvariantCultureIgnoreCase)) &&
        (!_filteringFlags[1] ||
         p.LastName.Contains(_filteringValues[1], StringComparison.InvariantCultureIgnoreCase)) &&
        (!_filteringFlags[2] || p.BirthDateString == _filteringValues[2]) &&
        (!_filteringFlags[3] || p.Email.Contains(_filteringValues[3], StringComparison.InvariantCultureIgnoreCase)) &&
        (!_filteringFlags[4] || _filteringValues[4] == "Yes" && p.IsAdult ||
         _filteringValues[4] == "No" && !p.IsAdult) &&
        (!_filteringFlags[5] || _filteringValues[5] == "Yes" && p.IsBornToday ||
         _filteringValues[5] == "No" && !p.IsBornToday) &&
        (!_filteringFlags[6] || $"{p.ChineseZodiacSign.name} {p.ChineseZodiacSign.emoji}" == _filteringValues[6]) &&
        (!_filteringFlags[7] || $"{p.WesternZodiacSign.name} {p.WesternZodiacSign.emoji}" == _filteringValues[7])
      ).ToList();
    }

    private async Task SerializeList()
    {
      await using var fs = File.Create(_path);
      await JsonSerializer.SerializeAsync(fs, PersonList);
    }

    private async Task<List<PersonModel>> DeserializeList()
    {
      await using var fs = File.OpenRead(_path);
      return await JsonSerializer.DeserializeAsync<List<PersonModel>>(fs);
    }
  }
}