using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkiveCollegeMotion.Data;
using SkiveCollegeMotion.Models;

namespace SkiveCollegeMotion.Pages.Tilmeldinger
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

        // Key is group name value is columns
        public IList<Aktivitet> Activities { get; set; }
        public IList<ApplicationUser> Students { get; set; }
        public IList<IGrouping<string, Tilmelding>> Tilmeldinger { get; set; }
        public string SortOrder { get; set; }

        public async Task OnGetAsync(string sort)
        {
            Students = await _userManager.GetUsersForClaimAsync(new System.Security.Claims.Claim("UserType", "Student"));

            if (Students.Count == 0)
            {
                // No students to show
                Tilmeldinger = new List<IGrouping<string, Tilmelding>>();
                return;
            }

            // Convert to lowercase for more forgiving manual parameter construction
            SortOrder = (sort ?? "aktivitet").ToLower();
            Activities = await _context.Aktivitet.ToListAsync();

            Activities.Add(new Aktivitet() {
                ID=0,
                Navn="Ikke tilmeldt",
                Sted=string.Empty,
                Antal=0,
                Ansvarlig=string.Empty
            });

            List<Tilmelding> tilmeldinger = await _context.Tilmelding.ToListAsync();

            IEnumerable<Tilmelding> missing = Students.Where(s => !tilmeldinger.Exists(t => t.Elev == s.UserName)).Select(s => new Tilmelding() { Elev = s.UserName, Aktivitet = 0 });
            tilmeldinger.AddRange(missing);

            tilmeldinger = tilmeldinger.OrderBy(t => {
                var student = Students.First(s => s.UserName == t.Elev);
                return $"{student.Fornavn} {student.Efternavn}";
            }).ToList();

            switch (SortOrder)
            {
                case "hold":
                    Tilmeldinger = tilmeldinger.GroupBy(t => Students.First(s => s.UserName == t.Elev).Hold).ToList();
                    break;
                // Default being "aktivitet"
                default:
                    Tilmeldinger = tilmeldinger.GroupBy(t => Activities.First(a => a.ID == t.Aktivitet).Navn).ToList();
                    break;
            }
        }
    }
}
