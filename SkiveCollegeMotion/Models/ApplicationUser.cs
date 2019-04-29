using Microsoft.AspNetCore.Identity;

namespace SkiveCollegeMotion.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Navn { get; set; }
    }
}