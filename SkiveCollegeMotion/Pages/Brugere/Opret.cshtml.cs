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

namespace SkiveCollegeMotion.Pages.Brugere
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
                    Fornavn = Bruger.Fornavn,
                    Efternavn = Bruger.Efternavn,
                    UserName = Bruger.Login,
                    Email = Bruger.Email
                };
                string password = Security.generatePassword();
                IdentityResult result = await _userManager.CreateAsync(bruger, password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddClaimAsync(bruger, new Claim("UserType", "Teacher"));
                    if (result.Succeeded)
                    {
                        await _emailSender.SendEmailAsync(bruger.Email, "Tilmeld motionshold på Skive College", $"Kære {bruger.Fornavn}<br><br>På Skive College er der motion Mandag og Torsdag, for at tilmelde dig skal du via følgende link: https://Studyweb.dk/Login tilmelde dig de aktiviteter du ønsker at følge det næste semester.<br><br>Du logger på med dit unilogin som brugernavn.<br><br>Din personlige adgangskode er: {password}.<br><br>Venlig hilsen<br><br>Motions teamet på Skive College");
                        return RedirectToPage("./Index");
                    }
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