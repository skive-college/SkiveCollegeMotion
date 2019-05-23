using Microsoft.AspNetCore.Identity;

namespace SkiveCollegeMotion.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }
        public string Hold { get; set; }
        public string ElevType { get; set; }
    }
}