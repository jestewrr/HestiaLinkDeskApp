using System;
using System.Collections.Generic;
using HestiaLink.Models;
using Microsoft.EntityFrameworkCore;

namespace HestiaLink.Data;

public partial class HestiaLinkContext : DbContext
{
    public HestiaLinkContext()
    {
<<<<<<< HEAD
=======
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

            // Housekeeping
            modelBuilder.Entity<CleaningTask>().ToTable("CleaningTask");

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
        }
>>>>>>> origin/master
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

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__8B69263C10AF54A1");

            entity.ToTable("Attendance");

            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.ActualCheckIn).HasColumnType("datetime");
            entity.Property(e => e.ActualCheckOut).HasColumnType("datetime");
            entity.Property(e => e.AttendanceStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsListed).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.OvertimeHours)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.RegularHours)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__Attendanc__Sched__3A4CA8FD");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__Bill__11F2FC4A0C399778");

            entity.ToTable("Bill");

            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.AmountPaid)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BalanceDue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BillDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.BillStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.GrandTotal)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.SubtotalAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Bills)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK__Bill__Reservatio__3B40CD36");
        });

        modelBuilder.Entity<BillDetail>(entity =>
        {
            entity.HasKey(e => e.BillDetailId).HasName("PK__BillDeta__793CAF758DE5B0D7");

            entity.ToTable("BillDetail");

            entity.Property(e => e.BillDetailId).HasColumnName("BillDetailID");
            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Bill).WithMany(p => p.BillDetails)
                .HasForeignKey(d => d.BillId)
                .HasConstraintName("FK__BillDetai__BillI__3C34F16F");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD72A825CC");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1FC47E03C");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.PositionId).HasColumnName("PositionID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK__Employee__Positi__3D2915A8");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.GuestId).HasName("PK__Guest__0C423C32E66F1C99");

            entity.ToTable("Guest");

            entity.Property(e => e.GuestId).HasColumnName("GuestID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.IncomeId).HasName("PK__Income__60DFC66CE5357291");

            entity.ToTable("Income");

            entity.Property(e => e.IncomeId).HasColumnName("IncomeID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IncomeType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

            entity.HasOne(d => d.Payment).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK__Income__PaymentI__3E1D39E1");
        });

        modelBuilder.Entity<InventoryConsumption>(entity =>
        {
            entity.HasKey(e => e.ConsumptionId).HasName("PK__Inventor__E3A1C43751518DAA");

            entity.ToTable("InventoryConsumption");

            entity.Property(e => e.ConsumptionId).HasColumnName("ConsumptionID");
            entity.Property(e => e.ConsumptionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InventoryItemId).HasColumnName("InventoryItemID");
            entity.Property(e => e.QuantityConsumed).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ServiceTransactionId).HasColumnName("ServiceTransactionID");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.InventoryConsumptions)
                .HasForeignKey(d => d.InventoryItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Inven__40058253");

            entity.HasOne(d => d.ServiceTransaction).WithMany(p => p.InventoryConsumptions)
                .HasForeignKey(d => d.ServiceTransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Servi__3F115E1A");
        });

        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Inventor__727E83EB6E16DF92");

            entity.ToTable("InventoryItem");

            entity.HasIndex(e => e.ItemCode, "UQ__Inventor__3ECC0FEA5A9AA4E5").IsUnique();

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentStock).HasDefaultValue(0);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ItemCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ItemName).HasMaxLength(255);
            entity.Property(e => e.ReorderPoint).HasDefaultValue(10);
            entity.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A58058424B9");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Completed");
            entity.Property(e => e.ReferenceNumber).HasMaxLength(100);

            entity.HasOne(d => d.Bill).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BillId)
                .HasConstraintName("FK__Payment__BillID__40F9A68C");

            entity.HasOne(d => d.ProcessedByNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ProcessedBy)
                .HasConstraintName("FK__Payment__Process__41EDCAC5");
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.HasKey(e => e.PayrollId).HasName("PK__Payroll__99DFC6923A42C9B5");

            entity.ToTable("Payroll");

            entity.Property(e => e.PayrollId).HasColumnName("PayrollID");
            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GrossPay)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HourlyRate)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IncomeId).HasColumnName("IncomeID");
            entity.Property(e => e.NetPay)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OvertimeHours)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OvertimePay)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RegularPay)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TaxAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxId).HasColumnName("TaxID");
            entity.Property(e => e.TotalHours)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Attendance).WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.AttendanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payroll__Attenda__42E1EEFE");

            entity.HasOne(d => d.Income).WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.IncomeId)
                .HasConstraintName("FK__Payroll__IncomeI__43D61337");

            entity.HasOne(d => d.Tax).WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.TaxId)
                .HasConstraintName("FK__Payroll__TaxID__44CA3770");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK__Position__60BB9A59CCAD4F03");

            entity.ToTable("Position");

            entity.Property(e => e.PositionId).HasColumnName("PositionID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.PayGrade).HasMaxLength(50);
            entity.Property(e => e.PositionLevel).HasMaxLength(50);
            entity.Property(e => e.PositionTitle).HasMaxLength(100);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.Department).WithMany(p => p.Positions)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Position__Depart__45BE5BA9");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Reservat__B7EE5F0414C10D58");

            entity.ToTable("Reservation");

            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GuestId).HasColumnName("GuestID");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Confirmed");
            entity.Property(e => e.TotalPayment).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Guest).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.GuestId)
                .HasConstraintName("FK__Reservati__Guest__46B27FE2");
        });

        modelBuilder.Entity<ReservedRoom>(entity =>
        {
            entity.HasKey(e => e.ReservedRoomId).HasName("PK__Reserved__7984BDDF98E06C47");

            entity.ToTable("ReservedRoom");

            entity.Property(e => e.ReservedRoomId).HasColumnName("ReservedRoomID");
            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");

            entity.HasOne(d => d.Reservation).WithMany(p => p.ReservedRooms)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK__ReservedR__Reser__47A6A41B");

            entity.HasOne(d => d.Room).WithMany(p => p.ReservedRooms)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__ReservedR__RoomI__489AC854");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__32863919CD3E4CBA");

            entity.ToTable("Room");

            entity.HasIndex(e => e.RoomNumber, "UQ__Room__AE10E07AF656F89E").IsUnique();

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.RoomNumber).HasMaxLength(20);
            entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Available");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomTypeId)
                .HasConstraintName("FK__Room__RoomTypeID__498EEC8D");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.RoomTypeId).HasName("PK__RoomType__BCC89611931E1F84");

            entity.ToTable("RoomType");

            entity.Property(e => e.RoomTypeId).HasColumnName("RoomTypeID");
            entity.Property(e => e.BasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active");
            entity.Property(e => e.TypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B69A047F57F");

            entity.ToTable("Schedule");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Schedule__Employ__4A8310C6");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__C51BB0EAD4FC11EF");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.InventoryNotes).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ServiceCategoryId).HasColumnName("ServiceCategoryID");
            entity.Property(e => e.ServiceName).HasMaxLength(100);
            entity.Property(e => e.StandardPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.UsesInventory).HasDefaultValue(false);

            entity.HasOne(d => d.ServiceCategory).WithMany(p => p.Services)
                .HasForeignKey(d => d.ServiceCategoryId)
                .HasConstraintName("FK__Service__Service__4B7734FF");
        });

        modelBuilder.Entity<ServiceCategory>(entity =>
        {
            entity.HasKey(e => e.ServiceCategoryId).HasName("PK__ServiceC__E4CC7E8A6A7C3617");

            entity.ToTable("ServiceCategory");

            entity.Property(e => e.ServiceCategoryId).HasColumnName("ServiceCategoryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Function).HasMaxLength(200);
            entity.Property(e => e.ServiceCategoryName).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ServiceInventory>(entity =>
        {
            entity.HasKey(e => e.ServiceInventoryId).HasName("PK__ServiceI__0EF88863A125CDB1");

            entity.ToTable("ServiceInventory");

            entity.Property(e => e.ServiceInventoryId).HasColumnName("ServiceInventoryID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InventoryItemId).HasColumnName("InventoryItemID");
            entity.Property(e => e.QuantityRequired)
                .HasDefaultValue(1.0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

            entity.HasOne(d => d.InventoryItem).WithMany(p => p.ServiceInventories)
                .HasForeignKey(d => d.InventoryItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceIn__Inven__4D5F7D71");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceInventories)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceIn__Servi__4C6B5938");
        });

        modelBuilder.Entity<ServiceTransaction>(entity =>
        {
            entity.HasKey(e => e.ServiceTransactionId).HasName("PK__ServiceT__1A17A062F07BC9A2");

            entity.ToTable("ServiceTransaction", tb => tb.HasTrigger("trg_AutoDeductInventory"));

            entity.Property(e => e.ServiceTransactionId).HasColumnName("ServiceTransactionID");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.ServiceStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Active");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TransactionDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Reservation).WithMany(p => p.ServiceTransactions)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK__ServiceTr__Reser__4E53A1AA");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceTransactions)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__ServiceTr__Servi__4F47C5E3");
        });

        modelBuilder.Entity<SystemUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__SystemUs__1788CCAC094D1D4C");

            entity.ToTable("SystemUser");

            entity.HasIndex(e => e.Username, "UQ__SystemUs__536C85E4ACFB976B").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.SystemUsers)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__SystemUse__Emplo__503BEA1C");
        });

        modelBuilder.Entity<CleaningTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Task__7C6949D1282BD6B3");

            entity.ToTable("Task");

            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Room).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task__RoomID__51300E55");

            entity.HasOne(d => d.User).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task__UserID__5224328E");
        });

        modelBuilder.Entity<Tax>(entity =>
        {
            entity.HasKey(e => e.TaxId).HasName("PK__Tax__711BE08C2FBF7846");

            entity.ToTable("Tax");

            entity.Property(e => e.TaxId).HasColumnName("TaxID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active");
            entity.Property(e => e.TaxDescription).HasMaxLength(500);
            entity.Property(e => e.TaxName).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<TotalIncome>(entity =>
        {
            entity.HasKey(e => e.IncomeId).HasName("PK__TotalInc__60DFC66C43AF1510");

            entity.ToTable("TotalIncome");

            entity.Property(e => e.IncomeId).HasColumnName("IncomeID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<VwAttendanceDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_AttendanceDetails");

            entity.Property(e => e.ActualCheckIn).HasColumnType("datetime");
            entity.Property(e => e.ActualCheckOut).HasColumnType("datetime");
            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.AttendanceStatus).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeName).HasMaxLength(101);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.OvertimeHours).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.RegularHours).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwBookingHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_BookingHistory");

            entity.Property(e => e.BookingDate).HasColumnType("datetime");
            entity.Property(e => e.BookingId)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.BookingStatus).HasMaxLength(50);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GuestName).HasMaxLength(101);
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);
            entity.Property(e => e.Room).HasMaxLength(20);
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<VwEmployeeFullDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_EmployeeFullDetails");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeStatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.PayGrade).HasMaxLength(50);
            entity.Property(e => e.PositionId).HasColumnName("PositionID");
            entity.Property(e => e.PositionLevel).HasMaxLength(50);
            entity.Property(e => e.PositionStatus).HasMaxLength(50);
            entity.Property(e => e.PositionTitle).HasMaxLength(100);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<VwEmployeePayrollSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_EmployeePayrollSummary");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeName).HasMaxLength(101);
            entity.Property(e => e.HourlyRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MonthlySalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.PositionTitle).HasMaxLength(100);
            entity.Property(e => e.TotalHours).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalOvertimeHours).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalRegularHours).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalSalary).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<VwRoomAvailability>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_RoomAvailability");

            entity.Property(e => e.BasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CurrentGuest).HasMaxLength(101);
            entity.Property(e => e.CurrentStatus).HasMaxLength(50);
            entity.Property(e => e.RoomNumber).HasMaxLength(20);
            entity.Property(e => e.TypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<VwRoomStatusDisplay>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_RoomStatusDisplay");

            entity.Property(e => e.Guest).HasMaxLength(101);
            entity.Property(e => e.RoomInfo).HasMaxLength(128);
            entity.Property(e => e.RoomNumber).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<VwRoomsBasic>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_RoomsBasic");

            entity.Property(e => e.BasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RoomNumber).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<VwRoomsNeedingCleaning>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_RoomsNeedingCleaning");

            entity.Property(e => e.RoomId)
                .ValueGeneratedOnAdd()
                .HasColumnName("RoomID");
            entity.Property(e => e.RoomNumber).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<VwTodaysRoomStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TodaysRoomStatus");

            entity.Property(e => e.Status).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
