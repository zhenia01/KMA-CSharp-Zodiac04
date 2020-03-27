using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BorodaikevychZodiac.Models;

namespace BorodaikevychZodiac.Services
{
  public enum PersonListSortingOption
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

    private readonly BitArray _sorting = new BitArray(8);

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

    public void Sort(PersonListSortingOption option)
    {
      int index = (int)option;
      _sorting[index] = !_sorting[index];
      PersonList.Sort((p1, p2) =>
        {
          int result = option switch
          {
            PersonListSortingOption.FirstName => p1.FirstName.CompareTo(p2.FirstName),
            PersonListSortingOption.LastName => p1.LastName.CompareTo(p2.LastName),
            PersonListSortingOption.BirthDate => p1.BirthDateDateTime.CompareTo(p2.BirthDateDateTime),
            PersonListSortingOption.Email => p1.Email.CompareTo(p2.Email),
            PersonListSortingOption.Adult => p1.IsAdult.CompareTo(p2.IsAdult),
            PersonListSortingOption.BirthDay => p1.IsBornToday.CompareTo(p2.IsBornToday),
            PersonListSortingOption.ChineseSign => p1.ChineseZodiacSign.name.CompareTo(p2.ChineseZodiacSign.name),
            PersonListSortingOption.WesternSign => p1.WesternZodiacSign.name.CompareTo(p2.WesternZodiacSign.name),
            _ => -1
          };
          return _sorting[index] ? result : -result;
        });
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