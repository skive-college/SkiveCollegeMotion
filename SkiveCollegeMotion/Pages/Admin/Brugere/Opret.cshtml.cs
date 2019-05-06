using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IEmailSender _emailSender;

        public OpretModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
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
                string password = Security.getNewPassword();
                IdentityResult result = await _userManager.CreateAsync(bruger, password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddClaimAsync(bruger, new Claim("UserType", "Teacher"));
                    if (result.Succeeded)
                    {
                        return RedirectToPage("./Index");
                    }
                    var callbackUrl = Url.Page(
                        "/Account/Login",
                        pageHandler: null,
                        values: new { area = "Identity" },
                        protocol: Request.Scheme);
                    string link = HtmlEncoder.Default.Encode(callbackUrl);
                    await _emailSender.SendEmailAsync(bruger.Email, "Motion", $"Din adgangskode er: {password}.<br>Du kan logge på ved at <a href='{link}'>klikke her</a>.");
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