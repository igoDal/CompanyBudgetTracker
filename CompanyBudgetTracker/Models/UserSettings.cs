using Microsoft.AspNetCore.Identity;

public class UserSettings
{
    public int Id { get; set; } 
    public string UserId { get; set; }
    public bool EnableNotifications { get; set; } = true;
    public string Language { get; set; } = "en-US";
    public string Theme { get; set; } = "Light";
    public virtual IdentityUser User { get; set; }
}