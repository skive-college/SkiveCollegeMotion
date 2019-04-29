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

namespace SkiveCollegeMotion.Pages.Admin
{
    public class EleverModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<ApplicationUser> _roleManager;

        public EleverModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            //_roleManager = roleManager;
        }

        public IList<ApplicationUser> Elev { get;set; }

        public async Task OnGetAsync()
        {
            Elev = await _userManager.Users.ToListAsync();
            //var s = _userManager.AddToRoleAsync(_userManager.FindByNameAsync("toke0344").Result,"Teacher").Result;
            ;
        }
    }
}
