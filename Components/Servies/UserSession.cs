namespace HestiaLink.Services
{
    public class UserSession
    {
        public int UserID { get; set; } = 0;
        public string Username { get; set; } = "Guest";
        public string Role { get; set; } = "Guest";
        public bool IsAuthenticated { get; set; } = false;

        public void Login(int userId, string username, string role)
        {
            UserID = userId;
            Username = username;
            Role = role;
            IsAuthenticated = true;
        }

        public void Logout()
        {
            UserID = 0;
            Username = "Guest";
            Role = "Guest";
            IsAuthenticated = false;
        }

        // Legacy support for old SetUser method (for components that still use it)
        public void SetUser(string username, string role)
        {
            Username = username;
            Role = role;
            IsAuthenticated = true;
        }
    }
}