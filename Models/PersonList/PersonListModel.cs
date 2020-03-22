using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BorodaikevychZodiac.Models.User;
using Microsoft.AspNetCore.Hosting;

namespace BorodaikevychZodiac.Models.PersonList
{
  public class PersonListModel
  {
    public List<UserModel> PersonList { get; } = new List<UserModel>();

    public async Task Initialize()
    {
      try
      {
        await using var fs = File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "Storage", "storage.json"));
        PersonList.AddRange(await JsonSerializer.DeserializeAsync<List<UserModel>>(fs));
      }
      catch (FileNotFoundException)
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

          PersonList[i].SetBirthDateStringAsync(sb.ToString()).Wait();

          await using var fs = File.Create(Path.Combine(Directory.GetCurrentDirectory(), "Storage", "storage.json"));
          await JsonSerializer.SerializeAsync(fs, PersonList);
        }
      }
    }
  }
}