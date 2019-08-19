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
    public class DetaljerModel : PageModel
    {
        private readonly SkiveCollegeMotion.Data.ApplicationDbContext _context;

        public DetaljerModel(SkiveCollegeMotion.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
