using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SkiveCollegeMotion.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Unilogin")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }
        public string Hold { get; set; }
        public string ElevType { get; set; }
    }
}