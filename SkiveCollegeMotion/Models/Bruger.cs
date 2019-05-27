using System;
using System.ComponentModel.DataAnnotations;

namespace SkiveCollegeMotion.Models
{
	public class Bruger
    {
        [Required]
        public string Navn { get; set; }
        
        [Key]
        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string Login { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
