﻿using System;
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
        public DbSet<SkiveCollegeMotion.Models.Aktivitet> Aktivitet { get; set; }
        public DbSet<SkiveCollegeMotion.Models.Bruger> Bruger { get; set; }
    }
}
