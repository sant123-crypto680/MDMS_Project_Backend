namespace MDMS_PRoject_Backend.Models;

public class Notification
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty; // FK to User
    public string Type { get; set; } = string.Empty; // bill_generated, payment_due, payment_received, high_consumption, maintenance
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public User? User { get; set; }
}
