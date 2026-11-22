using System.Text.Json.Serialization;

namespace MDMS_PRoject_Backend.Models;

public class User
{
    public string Id { get; set; } = string.Empty; // e.g. USR-001
    public string Role { get; set; } = "end_user"; // enterprise, zone_admin, end_user
    public string? ZoneId { get; set; } // FK to Zone (null for enterprise admin)
    public string? ZoneName { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string Status { get; set; } = "Active"; // Active, Deactivated, Pending
    public int MeterCount { get; set; } = 0;
    public string? NotificationPrefs { get; set; } // JSON string: { "email": true, "sms": false }
    public DateTime JoinedOn { get; set; } = DateTime.UtcNow;
    public DateTime? DeactivatedOn { get; set; }
    public string? AvatarUrl { get; set; }

    // Navigation Properties
    public Zone? Zone { get; set; }

    [JsonIgnore]
    public Zone? AdministeredZone { get; set; }

    [JsonIgnore]
    public ICollection<Meter> Meters { get; set; } = new List<Meter>();

    [JsonIgnore]
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    [JsonIgnore]
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [JsonIgnore]
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [JsonIgnore]
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
