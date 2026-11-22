using System.Text.Json.Serialization;

namespace MDMS_PRoject_Backend.Models;

public class Meter
{
    public string Id { get; set; } = string.Empty; // e.g. MTR-1001
    public string OwnerId { get; set; } = string.Empty; // FK to User
    public string? OwnerName { get; set; }
    public string ZoneId { get; set; } = string.Empty; // FK to Zone
    public string? ZoneName { get; set; }
    public string TariffId { get; set; } = string.Empty; // FK to Tariff
    public string? TariffCategory { get; set; }
    public string? Address { get; set; }
    public string Status { get; set; } = "Active"; // Active or Deactivated
    public bool IsPrimary { get; set; } = false;
    public double LastReading { get; set; } = 0; // kWh
    public DateTime? LastReadingAt { get; set; }
    public DateTime InstalledOn { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public User? Owner { get; set; }
    public Zone? Zone { get; set; }
    public Tariff? Tariff { get; set; }

    [JsonIgnore]
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    [JsonIgnore]
    public ICollection<MeterReading> Readings { get; set; } = new List<MeterReading>();

    [JsonIgnore]
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}
