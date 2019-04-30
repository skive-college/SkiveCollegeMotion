using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkiveCollegeMotion.Data;
using SkiveCollegeMotion.Models;

namespace SkiveCollegeMotion.Pages.Aktiviteter
{
    public class EditModel : PageModel
    {
        private readonly SkiveCollegeMotion.Data.ApplicationDbContext _context;

        public EditModel(SkiveCollegeMotion.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Aktivitet Aktivitet { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Aktivitet = await _context.Aktivitet.FirstOrDefaultAsync(m => m.ID == id);

            if (Aktivitet == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Aktivitet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AktivitetExists(Aktivitet.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AktivitetExists(int id)
        {
            return _context.Aktivitet.Any(e => e.ID == id);
        }
    }
}
