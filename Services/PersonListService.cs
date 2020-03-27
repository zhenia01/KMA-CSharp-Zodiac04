using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BorodaikevychZodiac.Models;

namespace BorodaikevychZodiac.Services
{
  public class PersonListService
  {
    public List<PersonModel> PersonList { get; private set; }

    private readonly string _path =
      Path.Combine(Directory.GetCurrentDirectory(), "Storage", "storage.json");

    public PersonListService()
    {
      try
      {
        using var fs = File.OpenRead(_path);
        PersonList = JsonSerializer.Deserialize<List<PersonModel>>(new StreamReader(fs).ReadToEnd());
      }
      catch (Exception e)
      {
        if (e is FileNotFoundException || e is JsonException)
        {
          PersonList = new List<PersonModel>();
          for (int i = 0; i < 50; i++)
          {
            PersonList.Add(new PersonModel
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

            PersonList[i].SetBirthDateStringAsync(sb.ToString()).Wait();
          }

          SerializeList().Wait();
        }
      }
    }


    public int Count => PersonList.Count;

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