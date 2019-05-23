using CsvHelper.Configuration.Attributes;
using System;

namespace SkiveCollegeMotion.Models
{
    public class Elev
    {
        [Index(0)]
        public string ElevType { get; set; }

        [Index(1)]
        public string Hold { get; set; }

        [Index(2)]
        public string Username { get; set; }

        [Index(3)]
        public string FirstName { get; set; }

        [Index(4)]
        public string LastName { get; set; }
    }
}