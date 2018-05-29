using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StandApi.Models;

namespace StandApi.Interfaces
{
    public interface IStandRepository
    {
        Task<List<StandEntry>> GetAllEntries();
        Task<List<StandEntry>> GetAllEntriesForStand(string url);
        Task<StandEntry> GetLastEntryForStand(string url);
        void AddEntry(StandEntry entry);
        void AddStand(Stand stand);
        Task SaveChanges();
    }
}
