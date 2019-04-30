using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkiveCollegeMotion.Data;
using SkiveCollegeMotion.Models;
using SkiveCollegeMotion.Utils;

namespace SkiveCollegeMotion.Pages.Admin.Brugere
{
    public class OpretModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OpretModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Bruger Bruger { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ModelState.IsValid)
            {
                ApplicationUser bruger = new ApplicationUser
                {
                    Navn = Bruger.Navn,
                    UserName = Bruger.Login,
                    Email = Bruger.Email
                };
                string password = Security.getNewPassword(8);
                IdentityResult result = await _userManager.CreateAsync(bruger, password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddClaimAsync(bruger, new Claim("UserType", "Teacher"));
                    if (result.Succeeded)
                    {
                        return RedirectToPage("./Index");
                    }

                    /*
                    string link = HtmlEncoder.Default.Encode("https://localhost:44341/Identity/Account/Login");
                    await _emailSender.SendEmailAsync(user.Email, "Motion", $"Din adgangskode er: {password}.<br>Du kan logge på ved at <a href='{link}'>klikke her</a>.");
                    */
                }
                // result variable contains the instance with error(s).
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}