using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace StandApi.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new StandContext(serviceProvider.GetRequiredService<DbContextOptions<StandContext>>()))
            {
                context.Database.Migrate();

                if (context.Stands.Any())
                {
                    return;
                }
                context.Stands.Add(new Stand { Url = "192.168.0.1", Name = "Stand1" });
                context.Stands.Add(new Stand { Url = "192.168.0.2", Name = "Stand2" });
                context.SaveChanges();

                context.StandEntries.Add(
                    new StandEntryDB
                    {
                        StandID = context.Stands.Where(stand => stand.Url == "192.168.0.1").First().ID,
                        Status = StandStatus.Working,
                        DateTime = DateTime.Parse("2018-05-27T10:00:00"),
                        Error = ""
                    }
                );
                context.StandEntries.Add(
                    new StandEntryDB
                    {
                        StandID = context.Stands.Where(stand => stand.Url == "192.168.0.2").First().ID,
                        Status = StandStatus.Crash,
                        DateTime = DateTime.Parse("2018-05-27T11:00:00"),
                        Error = "Segmentation fault"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
