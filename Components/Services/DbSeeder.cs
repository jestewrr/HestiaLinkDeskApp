using HestiaLink.Data;
using HestiaLink.Models;
using Microsoft.EntityFrameworkCore;

namespace HestiaLink.Services
{
    public class DbSeeder
    {
        private readonly HestiaLinkContext _context;

        public DbSeeder(HestiaLinkContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            try
            {
                // DATABASE MANAGEMENT DISABLED: Using external .bacpac import
                // await _context.Database.EnsureDeletedAsync();
                // await _context.Database.EnsureCreatedAsync();

                // Execute Database Scripts (DISABLED: Waiting for new SQL schema)
                // await ExecuteSqlScriptAsync("AddDepartmentIDToEmployee.sql");
                // await ExecuteSqlScriptAsync("CreateReservationSchema.sql");
                // await ExecuteSqlScriptAsync("CreateCleaningTaskTable.sql");
                // await ExecuteSqlScriptAsync("CreateHousekeepingViews.sql");
                // await ExecuteSqlScriptAsync("CreateMaintenanceRequestTable.sql");
                // await ExecuteSqlScriptAsync("CreateAttendanceTable.sql");
                // await ExecuteSqlScriptAsync("CreatePayrollSchema.sql");
                // await ExecuteSqlScriptAsync("CreateViews.sql");

                if (!await _context.SystemUsers.AnyAsync())
                {
                    var users = new List<SystemUser>
                    {
                        new SystemUser { Username = "admin", Password = "admin", Role = "Administrator", Status = "Active", CreatedAt = DateTime.Now },
                        new SystemUser { Username = "frontdesk", Password = "password", Role = "FrontDesk", Status = "Active", CreatedAt = DateTime.Now },
                        new SystemUser { Username = "housekeeping", Password = "password", Role = "Housekeeping", Status = "Active", CreatedAt = DateTime.Now },
                        new SystemUser { Username = "inventory", Password = "password", Role = "Inventory", Status = "Active", CreatedAt = DateTime.Now },
                        new SystemUser { Username = "hr", Password = "password", Role = "HR", Status = "Active", CreatedAt = DateTime.Now }
                    };

                    await _context.SystemUsers.AddRangeAsync(users);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding error: {ex.Message}");
            }
        }

        private async Task ExecuteSqlScriptAsync(string fileName)
        {
            try
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var filePath = Path.Combine(baseDir, "Database", fileName);

                if (!File.Exists(filePath))
                {
                    // Fallback for development if not copied to output
                    filePath = Path.Combine(@"c:\Users\Jester\source\repos\HestiaLinkDeskApp\Database", fileName);
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"SQL script not found: {filePath}");
                        return;
                    }
                }

                var sql = await File.ReadAllTextAsync(filePath);
                
                // Split by GO statements
                var commands = System.Text.RegularExpressions.Regex.Split(sql, @"^\s*GO\s*$", 
                    System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                foreach (var command in commands)
                {
                    if (!string.IsNullOrWhiteSpace(command))
                    {
                        await _context.Database.ExecuteSqlRawAsync(command);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing {fileName}: {ex.Message}");
            }
        }
    }
}
