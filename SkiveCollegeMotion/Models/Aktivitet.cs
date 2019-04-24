using System;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace SkiveCollegeMotion.Models
{
	public class Aktivitet
    {
        public int ID { get; set; }
        public string navn { get; set; }
        public string sted { get; set; }
        public int antal { get; set; }
        public string ansvarlig { get; set; }
    }
}
