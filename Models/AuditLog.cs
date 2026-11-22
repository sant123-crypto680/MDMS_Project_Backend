namespace MDMS_PRoject_Backend.Models;

public class AuditLog
{
    public string Id { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? UserId { get; set; } // FK to User
    public string? UserName { get; set; }
    public string Action { get; set; } = string.Empty; // e.g. Created zone, Deactivated meter
    public string Resource { get; set; } = string.Empty; // e.g. Zone ZN-002
    public string Status { get; set; } = "Success"; // Success, Failed

    // Navigation Properties
    public User? User { get; set; }
}
