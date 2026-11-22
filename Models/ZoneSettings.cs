using System.Text.Json.Serialization;

namespace MDMS_PRoject_Backend.Models;

public class ZoneSettings
{
    public string Id { get; set; } = string.Empty;
    public string ZoneId { get; set; } = string.Empty; // FK to Zone (1:1)
    public double HighConsumptionThreshold { get; set; }
    public double LowConsumptionThreshold { get; set; }
    public double AbnormalReadingFrequency { get; set; }
    public double InactiveMeterHours { get; set; }
    public bool NotifyEmail { get; set; } = true;
    public bool NotifySms { get; set; } = false;
    public bool NotifyInApp { get; set; } = true;

    // Navigation Properties
    [JsonIgnore]
    public Zone? Zone { get; set; }
}
