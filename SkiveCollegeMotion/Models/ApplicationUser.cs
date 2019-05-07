using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace SkiveCollegeMotion.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Brugernavn")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }
        public string Navn { get; set; }
        public string Hold { get; set; }
        public string ElevType { get; set; }
    }
}