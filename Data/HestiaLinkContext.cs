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

        // Housekeeping
        public DbSet<CleaningTask> CleaningTasks { get; set; }

        // Payroll related tables
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<TotalIncome> TotalIncomes { get; set; }
        public DbSet<Income> Incomes { get; set; }

        // ============================================
        // INVENTORY TABLES
        // ============================================
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<ServiceInventory> ServiceInventories { get; set; }
        public DbSet<InventoryConsumption> InventoryConsumptions { get; set; }

        // ============================================
        // SUPPLIER & PURCHASE TABLES
        // ============================================
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<InventoryPurchase> InventoryPurchases { get; set; }

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

            // New tables
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

            // Housekeeping - Map to Task table as specified
            modelBuilder.Entity<CleaningTask>().ToTable("Task");

            // Payroll related tables
            modelBuilder.Entity<Tax>().ToTable("Tax");
            modelBuilder.Entity<Payroll>().ToTable("Payroll");
            modelBuilder.Entity<TotalIncome>().ToTable("TotalIncome");
            modelBuilder.Entity<Income>().ToTable("Income");

            // ============================================
            // INVENTORY TABLES MAPPING
            // ============================================
            modelBuilder.Entity<InventoryItem>().ToTable("InventoryItem");
            modelBuilder.Entity<ServiceInventory>().ToTable("ServiceInventory");
            modelBuilder.Entity<InventoryConsumption>().ToTable("InventoryConsumption");

            // ============================================
            // SUPPLIER & PURCHASE TABLES MAPPING
            // ============================================
            modelBuilder.Entity<Supplier>().ToTable("Supplier");
            modelBuilder.Entity<InventoryPurchase>().ToTable("InventoryPurchase");

            // Supplier configuration
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasIndex(s => s.SupplierCode).IsUnique();
            });

            // InventoryPurchase configuration
            modelBuilder.Entity<InventoryPurchase>(entity =>
            {
                entity.HasIndex(p => p.PurchaseNumber).IsUnique();

                entity.HasOne(p => p.InventoryItem)
                    .WithMany()
                    .HasForeignKey(p => p.ItemID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Supplier)
                    .WithMany(s => s.InventoryPurchases)
                    .HasForeignKey(p => p.SupplierID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // InventoryItem-Supplier relationship
            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.HasOne(i => i.Supplier)
                    .WithMany(s => s.InventoryItems)
                    .HasForeignKey(i => i.SupplierID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Map database views (keyless entities)
            modelBuilder.Entity<EmployeePayrollSummaryView>()
                .ToView("vw_EmployeePayrollSummary")
                .HasNoKey();

            modelBuilder.Entity<EmployeeFullDetailsView>()
                .ToView("vw_EmployeeFullDetails")
                .HasNoKey();

            // ============================================
            // Configure Inventory Relationships
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

            // ============================================
            // Configure CleaningTask (Task) Relationships
            // ============================================
            modelBuilder.Entity<CleaningTask>(entity =>
            {
                entity.HasOne(ct => ct.Room)
                    .WithMany()
                    .HasForeignKey(ct => ct.RoomID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ct => ct.AssignedUser)
                    .WithMany()
                    .HasForeignKey(ct => ct.UserID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ============================================
            // Configure SystemUser Properties
            // ============================================
            modelBuilder.Entity<SystemUser>(entity =>
            {
                entity.Property(u => u.IsAvailable)
                    .HasDefaultValue(true);
            });
        }
    }
}