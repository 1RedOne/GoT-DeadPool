using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GameOfThronePool.Models;

namespace GameOfThronePool.Data
{
    public class DeadPoolDBContext : IdentityDbContext<ApplicationUser>
    {
        public DeadPoolDBContext(DbContextOptions<DeadPoolDBContext> options)
            : base(options)
        {
        }

        public DbSet<ShowCharacterStatusRecord> ShowCharacterStatusRecords { get; set; }
        public DbSet<UserCharacterSelection> UserCharacterSelections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ShowCharacterStatusRecord>().ToTable("ShowCharacterStatusRecord");
            builder.Entity<UserCharacterSelection>().ToTable("UserCharacterSelection");

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
