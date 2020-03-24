using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BorodaikevychZodiac.Models
{
  public static class PersonListModel
  {
    private static List<PersonModel> _list;

    static PersonListModel()
    {
      try
      {
        using var fs = File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Storage", "storage.json"));
        using var sr = new StreamReader(fs);
        _list = JsonSerializer.Deserialize<List<PersonModel>>(sr.ReadToEnd());
      }
      catch (FileNotFoundException)
      {
        _list = new List<PersonModel>();
        for (int i = 0; i < 50; i++)
        {
          _list.Add(new PersonModel
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

          _list[i].SetBirthDateStringAsync(sb.ToString()).Wait();

          using var fs = File.Create(Path.Combine(Directory.GetCurrentDirectory(), "Storage", "storage.json"));
          using var sw = new StreamWriter(fs);
          sw.Write(JsonSerializer.Serialize(_list));
        }
      }
    }

    public static int Count => _list.Count;

    public static async Task<PersonModel> GetPerson(int index)
    {
      await DeserializeList();
      return _list[index];
    }

    public static async Task SetPerson(int index, PersonModel person)
    {
      _list[index] = person;
      await SerializeList();
    }

    public static async Task DeletePerson(int index)
    {
      _list.RemoveAt(index);
      await SerializeList();
    }

    public static async Task AddPerson(PersonModel person)
    {
      _list.Add(person);
      await SerializeList();
    }

    private static async Task SerializeList()
    {
      await using var fs = File.Create(Path.Combine(Directory.GetCurrentDirectory(), "Storage", "storage.json"));
      await JsonSerializer.SerializeAsync(fs, _list);
    }
    
    private static async Task DeserializeList()
    {
      await using var fs = File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Storage", "storage.json"));
      _list = await JsonSerializer.DeserializeAsync<List<PersonModel>>(fs);
    }
  }
}