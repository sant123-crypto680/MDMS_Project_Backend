namespace MDMS_PRoject_Backend.Models;

public class Alert
{
    public string Id { get; set; } = string.Empty;
    public string ZoneId { get; set; } = string.Empty; // FK to Zone
    public string? ZoneName { get; set; }
    public string? MeterId { get; set; } // Optional FK to Meter
    public string Type { get; set; } = string.Empty; // High consumption, Inactive meter, Abnormal reading
    public string Severity { get; set; } = "Warning"; // Critical, Warning, Info
    public string Status { get; set; } = "Pending"; // Pending, Acknowledged, Resolved
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Zone? Zone { get; set; }
    public Meter? Meter { get; set; }
}
