using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Microsoft.AspNetCore.Identity;
using SkiveCollegeMotion.Utils;
using SkiveCollegeMotion.Data;
using SkiveCollegeMotion.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SkiveCollegeMotion.Pages.Elever
{
    public class ImporterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ImporterModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        [Required]
        [CsvFile]
        public IFormFile UploadedFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                using (StreamReader reader = new StreamReader(Request.Form.Files[0].OpenReadStream(), Encoding.UTF8, true))
                using (CsvReader csv = new CsvReader(reader))
                {
                    csv.Configuration.Delimiter = ";";
                    foreach (Elev record in csv.GetRecords<Elev>())
                    {
                        var user = new ApplicationUser
                        {
                            Fornavn = record.FirstName,
                            Efternavn = record.LastName,
                            UserName = record.Username,
                            Email = record.Username + "@skivecollege.dk",
                            Hold = record.Hold,
                            ElevType = record.ElevType
                        };
                        string password = Security.getNewPassword();

                        var callbackUrl = Url.Page(
                            "/Account/Login",
                            pageHandler: null,
                            values: new { area = "Identity" },
                            protocol: Request.Scheme);
                        string link = HtmlEncoder.Default.Encode(callbackUrl);
                        //await _emailSender.SendEmailAsync(user.Email, "Motion", $"Din adgangskode er: {password}.<br>Du kan logge på ved at <a href='{link}'>klikke her</a>.");

                        var result = await _userManager.CreateAsync(user, password);
                        if (result.Succeeded)
                        {
                            await _userManager.AddClaimAsync(user, new Claim("UserType", "Student"));
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}