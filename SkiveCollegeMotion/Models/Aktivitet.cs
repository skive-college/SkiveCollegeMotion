using System;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace SkiveCollegeMotion.Models
{
	public class Aktivitet
    {
        public int ID { get; set; }
        public string Navn { get; set; }
        public string Sted { get; set; }
        public int Antal { get; set; }
        public string Ansvarlig { get; set; }
    }
}
