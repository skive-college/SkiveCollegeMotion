using System;
using System.ComponentModel.DataAnnotations;

namespace SkiveCollegeMotion.Models
{
    /// <summary>
    /// For Scaffolding and validating ApplicationUser, not for direct use.
    /// </summary>
	public class Bruger
    {
        [Required]
        public string Fornavn { get; set; }
        [Required]
        public string Efternavn { get; set; }
        [Key]
        [Required]
        [StringLength(8, MinimumLength = 2)]
        public string Login { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
