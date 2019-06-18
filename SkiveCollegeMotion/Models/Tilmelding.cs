using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Summary description for Tilmelding
/// </summary>

namespace SkiveCollegeMotion.Models
{
    public class Tilmelding
    {
        public enum Day
        {
            Mandag,
            Torsdag
        }
        
        [Required]
        public string Elev { get; set; }
        
        [Required]
        public int Dag { get; set; }

        [Required]
        public int Aktivitet { get; set; }

    }
}
