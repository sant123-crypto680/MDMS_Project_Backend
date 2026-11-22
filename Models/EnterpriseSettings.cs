namespace MDMS_PRoject_Backend.Models;

public class EnterpriseSettings
{
    public string Id { get; set; } = "1"; // Singleton
    public int DataRetentionDays { get; set; } = 365;
    public int AutoLogoutMinutes { get; set; } = 30;
    public int AuditLogRetentionDays { get; set; } = 180;
    public string TimeZone { get; set; } = "Asia/Kolkata";
    public string DefaultLanguage { get; set; } = "en";
    public string Currency { get; set; } = "INR";
    public string? NotificationDefaults { get; set; } // e.g. "{\"email\":true,\"sms\":true,\"inApp\":true}"
}
