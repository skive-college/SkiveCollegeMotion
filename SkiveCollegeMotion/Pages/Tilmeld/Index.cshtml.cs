using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkiveCollegeMotion.Data;
using SkiveCollegeMotion.Models;

namespace SkiveCollegeMotion.Pages.Tilmeld
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Aktivitet> Aktivitet { get; set; }

        public int Valg { get; set; }

        public async Task OnGetAsync()
        {
            Aktivitet = await _context.Aktivitet.ToListAsync();

            string elev = (await _userManager.GetUserAsync(User)).UserName;
            Tilmelding current = await _context.Tilmelding.FirstOrDefaultAsync(t => t.Elev == elev);
            if (current != null)
            {
                Valg = current.Aktivitet;
            }
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            Valg = int.Parse(Request.Form["Valg"]);
            // Handle attempt at choosing nonexistant activity
            if (await _context.Aktivitet.AnyAsync(a => a.ID == Valg))
            {
                ModelState.AddModelError(string.Empty, "Den valgte aktivitet findes ikke");
                return Page();
            }
            Aktivitet = await _context.Aktivitet.ToListAsync();
            if (ModelState.IsValid)
            {
                int remaining = await _context.Tilmelding.CountAsync(t => t.Aktivitet == Valg);
                if (remaining < 1)
                {
                    ModelState.AddModelError(string.Empty, "Holdet er desværre fuldtegnet");
                }
                else
                {
                    var current = (await _context.Tilmelding.FindAsync((await _userManager.GetUserAsync(User)).UserName));
                    if (current != null)
                    {
                        // Already exists
                        if (Valg == current.Aktivitet)
                        {
                            // Already correct, do nothing
                            TempData["success"] = "Du er allerede meldt til " + (await _context.Aktivitet.FirstOrDefaultAsync(a => a.ID == Valg)).Navn;
                        }
                        else
                        {
                            // Change selection
                            string oldActivity = (await _context.Aktivitet.FirstOrDefaultAsync(a => a.ID == current.Aktivitet)).Navn;
                            string newActivity = (await _context.Aktivitet.FirstOrDefaultAsync(a => a.ID == Valg)).Navn;

                            _context.Tilmelding.Attach(current);
                            current.Aktivitet = Valg;
                            TempData["success"] = $"Din tilmelding er blevet skiftet fra {oldActivity} til {newActivity}";
                        }
                    }
                    else
                    {
                        // Doesnt exist, create new
                        _context.Tilmelding.Add(new Tilmelding()
                        {
                            Elev = (await _userManager.GetUserAsync(User)).UserName,
                            Aktivitet = Valg
                        });
                        // Add 1 cause ID is 1-based as opposed to everything else being 0-based
                        TempData["success"] = "Du er nu tilmeldt til " + (await _context.Aktivitet.FirstOrDefaultAsync(a => a.ID == Valg)).Navn;
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return Page();
        }
    }
}
