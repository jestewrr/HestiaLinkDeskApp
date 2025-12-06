using HestiaLink.Data;
using HestiaLink.Models;
using Microsoft.EntityFrameworkCore;

namespace HestiaLink.Services
{
    public class AuthenticationService
    {
        private readonly HestiaLinkContext _context;

        public AuthenticationService(HestiaLinkContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Validates user credentials against the SystemUser table
        /// </summary>
        public async Task<(bool IsValid, SystemUser? User, string Message)> AuthenticateAsync(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return (false, null, "Username and password are required");
                }

                // Find the user by username (case-insensitive)
                var user = await _context.SystemUsers
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower() && u.Status == "Active");

                if (user == null)
                {
                    return (false, null, "Invalid username or password");
                }

                // For production, implement proper password hashing (e.g., bcrypt)
                // For now, comparing plain text (NOT RECOMMENDED FOR PRODUCTION)
                if (user.Password != password)
                {
                    return (false, null, "Invalid username or password");
                }

                return (true, user, "Authentication successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication error: {ex.Message}");
                return (false, null, "An error occurred during authentication. Please try again.");
            }
        }

        /// <summary>
        /// Gets the appropriate landing page based on user role
        /// </summary>
        public string GetLandingPageForRole(string role)
        {
            return role?.ToLower() switch
            {
                "administrator" => "/dashboard",
                "manager" => "/dashboard",
                "frontdesk" => "/reservations/booking-history",
                "housekeeping" => "/housekeeping/room-status",
                "inventory" => "/inventory/stock-management",
                "hr" => "/hr/employee-management",
                _ => "/dashboard"
            };
        }
    }
}
