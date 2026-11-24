namespace MDMS_PRoject_Backend.Models;

public class ConsumptionSeries
{
    public string Id { get; set; } = string.Empty;
    public string Scope { get; set; } = "user"; // user, zone, enterprise
    public string ScopeId { get; set; } = string.Empty; // user id, zone id, or "ALL"
    public string Range { get; set; } = "month"; // day, week, month, year
    
    // Time interval labels (e.g. ["Day 01", "Day 02", ..., "Day 30"] or ["Mon", "Tue"])
    public List<string> Labels { get; set; } = new();
    
    // Data points for the current period
    public List<double> Current { get; set; } = new();
    
    // Data points for the previous comparative period
    public List<double> Previous { get; set; } = new();

    // Summary Metrics
    public double TotalConsumption { get; set; } = 0;   // Sum of consumption in period (kWh)
    public double AverageConsumption { get; set; } = 0; // Average consumption in period (kWh)
    public double MaxUsage { get; set; } = 0;           // Peak usage recorded in period (kWh)
    public double MinUsage { get; set; } = 0;           // Lowest usage recorded in period (kWh)
}
