using CsvHelper.Configuration.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SkiveCollegeMotion.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class LoginViewModel
    {
        #region Properties  
        
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        #endregion
    }
}