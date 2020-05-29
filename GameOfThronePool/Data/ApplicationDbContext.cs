﻿using System;
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


        protected override void OnModelCreating(ModelBuilder builder)
        {
                        
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }


        public DbSet<GameOfThronePool.Models.ShowCharacterStatusRecord> ShowCharacterStatusRecord { get; set; }
        public DbSet<GameOfThronePool.Models.UserCharacterSelection> UserCharacterSelection { get; set; }
        public DbSet<GameOfThronePool.Models.UserBonusQuestion> UserBonusQuestion { get; set; }
        public DbSet<GameOfThronePool.Models.UserScoreRecord> UserScoreRecord { get; set; }
        public DbSet<CorrectAnswers> CorrectAnswers { get; set; }
        public DbSet<rightWhiteWalkers> rightWhiteWalkers { get; set; }
        public DbSet<wrongWhiteWalkers> wrongWhiteWalkers { get; set; }
        public DbSet<BonusQuestions> BonusQuestions { get; set; }
    }
}
