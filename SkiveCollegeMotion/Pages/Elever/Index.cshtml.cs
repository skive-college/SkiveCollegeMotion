using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkiveCollegeMotion.Models;
using SkiveCollegeMotion.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SkiveCollegeMotion.Pages.Elever
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

        public IList<ApplicationUser> Elev { get; set; }

        public async Task OnGetAsync()
        {
            Elev = await _userManager.GetUsersForClaimAsync(new Claim("UserType", "Student"));
        }
    }
}
