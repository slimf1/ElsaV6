using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccess
{
    public class ElsaContext : DbContext
    {
        public virtual DbSet<UserModel> Users { get; set; }
        public virtual DbSet<BadgeModel> Badges { get; set; }

        public ElsaContext()
        {
        }

        public ElsaContext(DbContextOptions<ElsaContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasKey(u => u.UserID);
            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder.Entity<BadgeModel>().ToTable("Badges");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(Utils.ReadFile(Path.Combine("Resources", "ConnectionString.txt")));
            }
        }
    }
}
