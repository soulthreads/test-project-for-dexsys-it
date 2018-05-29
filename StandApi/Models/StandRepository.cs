using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StandApi.Interfaces;

namespace StandApi.Models
{
    public class StandRepository : IStandRepository
    {
        private readonly StandContext _context;

        public StandRepository(StandContext context)
        {
            _context = context;
        }

        private StandEntry ConvertEntry(StandEntryDB e)
        {
            return new StandEntry {
                Url = e.Stand.Url,
                Status = e.Status,
                DateTime = e.DateTime,
                Error = e.Error
            };
        }

        public Task<List<StandEntry>> GetAllEntries()
        {
            return Task.FromResult(_context.StandEntries
                .Include(e => e.Stand)
                .Select(ConvertEntry)
                .ToList());
        }

        public Task<List<StandEntry>> GetAllEntriesForStand(string url)
        {
            return Task.FromResult(_context.StandEntries
                .Include(e => e.Stand)
                .Where(e => e.Stand.Url == url)
                .Select(ConvertEntry).ToList());
        }

        public Task<StandEntry> GetLastEntryForStand(string url)
        {
            return _context.StandEntries
                .Include(e => e.Stand)
                .Where(e => e.Stand.Url == url)
                .OrderBy(e => e.DateTime)
                .LastAsync()
                .ContinueWith(e => {
                    if (e.IsFaulted)
                        return null;
                    else
                        return ConvertEntry(e.Result);
                });
        }

        public void AddEntry(StandEntry entry)
        {
            _context.StandEntries.Add(
                new StandEntryDB
                {
                    StandID = _context.Stands.Single(stand => stand.Url == entry.Url).ID,
                    Status = entry.Status,
                    DateTime = entry.DateTime,
                    Error = entry.Error
                }
            );
        }

        public void AddStand(Stand stand)
        {
            _context.Stands.Add(stand);
        }

        public Task SaveChanges()
        {
            return _context.SaveChangesAsync();
        }
    }
}
