using System.Text.Json.Serialization;

namespace MDMS_PRoject_Backend.Models;

public class Zone
{
    public string Id { get; set; } = string.Empty; // e.g. ZN-001
    public string Name { get; set; } = string.Empty; // e.g. Mangalore
    public string? Location { get; set; }
    public string? Pincode { get; set; }
    public string? Description { get; set; }
    public string? AdminId { get; set; } // FK to User
    public string? AdminName { get; set; }
    public string Status { get; set; } = "Active"; // Active or Deactivated
    public int TotalMeters { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public User? Admin { get; set; }
    
    [JsonIgnore]
    public ICollection<User> Users { get; set; } = new List<User>();
    
    [JsonIgnore]
    public ICollection<Meter> Meters { get; set; } = new List<Meter>();
    
    [JsonIgnore]
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    
    [JsonIgnore]
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    
    public ZoneSettings? Settings { get; set; }
}
