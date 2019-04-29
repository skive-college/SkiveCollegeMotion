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
    public class IndexModel : PageModel
    {
        private readonly SkiveCollegeMotion.Data.ApplicationDbContext _context;

        public IndexModel(SkiveCollegeMotion.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Aktivitet> Aktivitet { get;set; }

        public async Task OnGetAsync()
        {
            Aktivitet = await _context.Aktivitet.ToListAsync();
        }
    }
}
