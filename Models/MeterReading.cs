namespace MDMS_PRoject_Backend.Models;

public class MeterReading
{
    public string Id { get; set; } = string.Empty;
    public string MeterId { get; set; } = string.Empty; // FK to Meter
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Granularity { get; set; } = "hourly"; // hourly, daily
    public double Reading { get; set; } // Cumulative reading (kWh)
    public double Consumption { get; set; } // Difference from previous reading (kWh)
    public string? Note { get; set; } // e.g. "Peak usage"

    // Navigation Properties
    public Meter? Meter { get; set; }
}
