using Microsoft.AspNetCore.Identity;

public class UserSettings
{
    public int Id { get; set; } 
    public string UserId { get; set; }
    public bool EnableNotifications { get; set; } = true;
    public bool NotifyByEmail { get; set; } = true;
    public bool NotifyBySMS { get; set; } = false;
    public bool NotifyInApp { get; set; } = true;
    public bool NotifyOnNewMessage { get; set; } = true;
    public bool NotifyOnTaskCompletion { get; set; } = true;
    public bool NotifyOnDueDateApproach { get; set; } = true;
    public string Language { get; set; } = "en-US";
    public string Theme { get; set; } = "Light";
}