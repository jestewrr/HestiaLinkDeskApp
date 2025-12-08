using HestiaLink.Models;
using HestiaLink.Models.Views;
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
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        // Payroll related tables
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<TotalIncome> TotalIncomes { get; set; }

        // CHANGED FROM PaymentIncome TO Income
        public DbSet<Income> Incomes { get; set; }  // This is the replacement for PaymentIncome

        // ============================================
        // NEW INVENTORY TABLES
        // ============================================
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<ServiceInventory> ServiceInventories { get; set; }
        public DbSet<InventoryConsumption> InventoryConsumptions { get; set; }

        // Database Views (keyless entities)
        public DbSet<EmployeePayrollSummaryView> EmployeePayrollSummaryView { get; set; }
        public DbSet<EmployeeFullDetailsView> EmployeeFullDetailsView { get; set; }

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
            modelBuilder.Entity<Attendance>().ToTable("Attendance");
            modelBuilder.Entity<Schedule>().ToTable("Schedule");

            // Payroll related tables
            modelBuilder.Entity<Tax>().ToTable("Tax");
            modelBuilder.Entity<Payroll>().ToTable("Payroll");
            modelBuilder.Entity<TotalIncome>().ToTable("TotalIncome");

            // CHANGED FROM PaymentIncome TO Income
            modelBuilder.Entity<Income>().ToTable("Income");  // This is the replacement for PaymentIncome

            // ============================================
            // NEW INVENTORY TABLES MAPPING
            // ============================================
            modelBuilder.Entity<InventoryItem>().ToTable("InventoryItem");
            modelBuilder.Entity<ServiceInventory>().ToTable("ServiceInventory");
            modelBuilder.Entity<InventoryConsumption>().ToTable("InventoryConsumption");

            // Map database views (keyless entities)
            modelBuilder.Entity<EmployeePayrollSummaryView>()
                .ToView("vw_EmployeePayrollSummary")
                .HasNoKey();

            modelBuilder.Entity<EmployeeFullDetailsView>()
                .ToView("vw_EmployeeFullDetails")
                .HasNoKey();

            // ============================================
            // OPTIONAL: Configure Inventory Relationships
            // ============================================
            modelBuilder.Entity<ServiceInventory>()
                .HasOne(si => si.Service)
                .WithMany()
                .HasForeignKey(si => si.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceInventory>()
                .HasOne(si => si.InventoryItem)
                .WithMany()
                .HasForeignKey(si => si.InventoryItemID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InventoryConsumption>()
                .HasOne(ic => ic.ServiceTransaction)
                .WithMany()
                .HasForeignKey(ic => ic.ServiceTransactionID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InventoryConsumption>()
                .HasOne(ic => ic.InventoryItem)
                .WithMany()
                .HasForeignKey(ic => ic.InventoryItemID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}