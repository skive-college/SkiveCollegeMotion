using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkiveCollegeMotion.Data;
using SkiveCollegeMotion.Models;

namespace SkiveCollegeMotion.Pages.Aktiviteter
{
    public class CreateModel : PageModel
    {
        private readonly SkiveCollegeMotion.Data.ApplicationDbContext _context;

        public CreateModel(SkiveCollegeMotion.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Aktivitet Aktivitet { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Aktivitet.Add(Aktivitet);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}