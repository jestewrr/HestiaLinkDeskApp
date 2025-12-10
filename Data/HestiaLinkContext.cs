using System;
using System.Collections.Generic;
using HestiaLink.Models;
using HestiaLink.Models.Views;
using Microsoft.EntityFrameworkCore;

namespace HestiaLink.Data;

public partial class HestiaLinkContext : DbContext
{
    public HestiaLinkContext()
    {

    }

    public HestiaLinkContext(DbContextOptions<HestiaLinkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<BillDetail> BillDetails { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Income> Incomes { get; set; }

    public virtual DbSet<InventoryConsumption> InventoryConsumptions { get; set; }

    public virtual DbSet<InventoryItem> InventoryItems { get; set; }

    public virtual DbSet<OperationalExpense> OperationalExpenses { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Payroll> Payrolls { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<ReservedRoom> ReservedRooms { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }

    public virtual DbSet<ServiceInventory> ServiceInventories { get; set; }

    public virtual DbSet<ServiceTransaction> ServiceTransactions { get; set; }

    public virtual DbSet<SystemUser> SystemUsers { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<InventoryPurchase> InventoryPurchases { get; set; }


    


    public virtual DbSet<CleaningTask> Tasks { get; set; }

    public virtual DbSet<Tax> Taxes { get; set; }

    public virtual DbSet<TotalIncome> TotalIncomes { get; set; }

    public virtual DbSet<VwAttendanceDetail> VwAttendanceDetails { get; set; }

    public virtual DbSet<VwBookingHistory> VwBookingHistories { get; set; }

    public virtual DbSet<VwEmployeeFullDetail> VwEmployeeFullDetails { get; set; }

    public virtual DbSet<VwEmployeePayrollSummary> VwEmployeePayrollSummaries { get; set; }

    public virtual DbSet<VwRoomAvailability> VwRoomAvailabilities { get; set; }

    public virtual DbSet<VwRoomStatusDisplay> VwRoomStatusDisplays { get; set; }

    public virtual DbSet<VwRoomsBasic> VwRoomsBasics { get; set; }

    public virtual DbSet<VwRoomsNeedingCleaning> VwRoomsNeedingCleanings { get; set; }

    public virtual DbSet<VwTodaysRoomStatus> VwTodaysRoomStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=JESTER-PC\\SQLEXPRESS;Initial Catalog=\"IT13 (1)\";Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OperationalExpense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId);
            entity.ToTable("OperationalExpenses");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExpenseDate).HasColumnType("datetime");
        });
        
        // ============================================
        // SUPPLIER & PURCHASE TABLES MAPPING
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

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
