namespace HestiaIT13Final.Shared.Services;

public class UserSession
{
    public int? CurrentUserId { get; set; }
    public string? CurrentUsername { get; set; }
    public bool IsAuthenticated => CurrentUserId.HasValue;
    
    public void Login(int userId, string username)
    {
        CurrentUserId = userId;
        CurrentUsername = username;
    }
    
    public void Logout()
    {
        CurrentUserId = null;
        CurrentUsername = null;
    }
}