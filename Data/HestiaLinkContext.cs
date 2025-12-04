using HestiaLink.Models;
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
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Service> Services { get; set; }

        // Added sets
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservedRoom> ReservedRooms { get; set; }
        public DbSet<ServiceTransaction> ServiceTransactions { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This tells Entity Framework to use your existing SQL Server table
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Position>().ToTable("Position");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<SystemUser>().ToTable("SystemUser");
            modelBuilder.Entity<Room>().ToTable("Room");
            modelBuilder.Entity<RoomType>().ToTable("RoomType");
            modelBuilder.Entity<ServiceCategory>().ToTable("ServiceCategory");
            modelBuilder.Entity<Service>().ToTable("Service");

            // New tables - assume names
            modelBuilder.Entity<Guest>().ToTable("Guest");
            modelBuilder.Entity<Reservation>().ToTable("Reservation");
            modelBuilder.Entity<ReservedRoom>().ToTable("ReservedRoom");
            modelBuilder.Entity<ServiceTransaction>().ToTable("ServiceTransaction");
            modelBuilder.Entity<Bill>().ToTable("Bill");
            modelBuilder.Entity<BillDetail>().ToTable("BillDetail");
            modelBuilder.Entity<Payment>().ToTable("Payment");
            modelBuilder.Entity<MaintenanceRequest>().ToTable("MaintenanceRequest");
        }
    }
}