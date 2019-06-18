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

namespace SkiveCollegeMotion.Pages
{
    public class TilmeldModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TilmeldModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Aktivitet> Aktivitet { get; set; }

        public int[] Valg { get; set; }

        public int DayCount { get; set; }

        public async Task OnGetAsync()
        {
            Aktivitet = await _context.Aktivitet.ToListAsync();

            string elev = (await _userManager.GetUserAsync(User)).UserName;
            DayCount = Enum.GetNames(typeof(Tilmelding.Day)).Length;
            Valg = new int[DayCount];
            for (int i = 0; i < DayCount; i++)
            {
                Tilmelding current = await _context.Tilmelding.FirstOrDefaultAsync(t => (t.Elev == elev) && (t.Dag == i));
                if (current != null)
                {
                    Valg[i] = current.Aktivitet;
                }/*
                else
                {
                    Valg[i] = 0;
                }*/
            }
            //return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            Aktivitet = await _context.Aktivitet.ToListAsync();
            DayCount = Enum.GetNames(typeof(Tilmelding.Day)).Length;
            Valg = new int[DayCount];
            for (int i = 0; i < DayCount; i++)
            {
                int.TryParse(Request.Form[$"Valg[{i}]"], out Valg[i]);
            }

            var s = Valg;

            if (ModelState.IsValid)
            {
                for (int i = 0; i < DayCount; i++)
                {
                    Aktivitet valgtAktivitet = await _context.Aktivitet.FindAsync(Valg[i]);

                    string dayName = Enum.GetName(typeof(Tilmelding.Day), i);

                    // Handle attempt at choosing nonexistant activity
                    if (valgtAktivitet == null)
                    {
                        ModelState.AddModelError(string.Empty, $"Den valgte aktivitet til på {dayName} findes ikke");
                        continue;
                    }

                    if ((await GetRemainingAsync(valgtAktivitet, i)) < 1)
                    {
                        ModelState.AddModelError(string.Empty, $"Holdet til {valgtAktivitet.Navn} på {dayName} er desværre fuldtegnet");
                    }
                    else
                    {
                        var userName = _userManager.GetUserName(User);
                        var current = (await _context.Tilmelding.FirstOrDefaultAsync(t => (t.Elev == userName) && (t.Dag == i)));
                        if (current != null)
                        {
                            // Already exists
                            if (Valg[i] == current.Aktivitet)
                            {
                                // Already correct, do nothing
                                TempData["success" + i] = $"Du er allerede meldt til {valgtAktivitet.Navn} på {dayName}";
                            }
                            else
                            {
                                // Change selection
                                string oldActivity = (await _context.Aktivitet.FirstOrDefaultAsync(a => a.ID == current.Aktivitet)).Navn;

                                _context.Tilmelding.Attach(current);
                                current.Aktivitet = Valg[i];
                                TempData["success" + i] = $"Din tilmelding for {dayName} er blevet skiftet fra {oldActivity} til {valgtAktivitet.Navn}";
                            }
                        }
                        else
                        {
                            // Doesnt exist, create new
                            _context.Tilmelding.Add(new Tilmelding()
                            {
                                Elev = userName,
                                Aktivitet = Valg[i],
                                Dag = i
                            });
                            // Add 1 cause ID is 1-based as opposed to everything else being 0-based
                            TempData["success" + i] = $"Du er nu tilmeldt til {valgtAktivitet.Navn} på {dayName}";
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return Page();
        }

        public async Task<int> GetRemainingAsync(Aktivitet aktivitet, int dag)
        {
            return aktivitet.Antal - await _context.Tilmelding.CountAsync(t => (t.Aktivitet == aktivitet.ID) && (t.Dag == dag));
        }
    }
}
