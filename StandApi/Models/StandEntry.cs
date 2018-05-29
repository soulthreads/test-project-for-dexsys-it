using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StandApi.Models
{
    public class Stand
    {
        public int ID { get; set; }

        [Required, DataType(DataType.Url)]
        public string Url { get; set; }
        public string Name { get; set; }

        public List<StandEntryDB> Items { get; set; }
    }

    public enum StandStatus
    {
        Disabled, StartingUp, Working, Crash
    }

    public class StandEntryDB
    {
        public int ID { get; set; }

        [Required]
        public int StandID { get; set; }
        public Stand Stand { get; set; }

        [Required]
        public StandStatus Status { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public string Error { get; set; }
    }

    public class StandEntry
    {
        public string Url { get; set; }
        public StandStatus Status { get; set; }
        public DateTime DateTime { get; set; }
        public string Error { get; set; }
    }
}
