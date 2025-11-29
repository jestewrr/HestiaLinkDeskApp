using HestiaLink.Models;  // Fixed namespace
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HestiaLink.Data
{
    public class HestiaLinkContext : DbContext
    {
        public HestiaLinkContext(DbContextOptions<HestiaLinkContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This tells Entity Framework to use your existing SQL Server table
            modelBuilder.Entity<Department>().ToTable("Department");
        }
    }
}