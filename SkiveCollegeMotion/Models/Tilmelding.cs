using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for Tilmelding
/// </summary>

namespace SkiveCollegeMotion.Models
{
    public class Tilmelding
    {
        [Key]
        [Required]
        public string Elev { get; set; }

        [Required]
        public int Aktivitet { get; set; }

    }
}
