using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SkiveCollegeMotion.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            string userType = User.Claims.FirstOrDefault(c => c.Type == "UserType").Value;
            if (userType == "Teacher" || userType == "SuperUser")
            {
                return Page();
            }
            else if (userType == "Student")
            {
                return RedirectToPage("./Tilmeld");
            }
            else
            {
                // Should not be possible as authorization is required to even get this far.
                // Someone either has no UserType or the value is not recognized
                return Forbid();
            }
        }
    }
}
