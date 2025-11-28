namespace HestiaLink.Services
{
    public class UserSession
    {
        public string Username { get; set; } = "Guest";
        public string Role { get; set; } = "Guest";

        public void SetUser(string username, string role)
        {
            Username = username;
            Role = role;
        }
    }
}