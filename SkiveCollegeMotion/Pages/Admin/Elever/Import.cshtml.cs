﻿using System;
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

namespace SkiveCollegeMotion.Pages.Admin.Elever
{
    [Authorize(Policy = "Admin")]
    public class ImportModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ImportModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
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
                using (StreamReader reader = new StreamReader(Request.Form.Files[0].OpenReadStream(), System.Text.Encoding.UTF8, true))
                using (CsvReader csv = new CsvReader(reader))
                {
                    csv.Configuration.Delimiter = ";";
                    foreach (Elev record in csv.GetRecords<Elev>())
                    {
                        var user = new ApplicationUser
                        {
                            Navn = record.FirstName + " " + record.LastName,
                            UserName = record.Username,
                            Email = record.Username + "@skivecollege.dk"
                        };
                        string password = Security.getNewPassword(8);
                        var result = await _userManager.CreateAsync(user, password);
                        if(user.UserName == "toke0344")
                        {
                            await _userManager.AddClaimAsync(user, new Claim("UserType", "Teacher"));
                            /*
                            string link = HtmlEncoder.Default.Encode("https://localhost:44341/Identity/Account/Login");
                            await _emailSender.SendEmailAsync(user.Email, "Motion", $"Din adgangskode er: {password}.<br>Du kan logge på ved at <a href='{link}'>klikke her</a>.");
                            */
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}