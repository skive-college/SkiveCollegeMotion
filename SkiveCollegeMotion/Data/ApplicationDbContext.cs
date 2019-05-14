using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkiveCollegeMotion.Models;

namespace SkiveCollegeMotion.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Aktivitet> Aktivitet { get; set; }
        public DbSet<Bruger> Bruger { get; set; }
        public DbSet<Tilmelding> Tilmelding { get; set; }
    }
}
