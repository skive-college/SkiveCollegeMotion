using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkiveCollegeMotion.Data;
using SkiveCollegeMotion.Models;

namespace SkiveCollegeMotion.Pages.Aktiviteter
{
    public class DeleteModel : PageModel
    {
        private readonly SkiveCollegeMotion.Data.ApplicationDbContext _context;

        public DeleteModel(SkiveCollegeMotion.Data.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Aktivitet = await _context.Aktivitet.FindAsync(id);

            if (Aktivitet != null)
            {
                _context.Aktivitet.Remove(Aktivitet);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
