namespace MDMS_PRoject_Backend.Models;

public class ConsumptionSeries
{
    public string Id { get; set; } = string.Empty;
    public string Scope { get; set; } = "user"; // user, zone, enterprise
    public string ScopeId { get; set; } = string.Empty; // user id, zone id, or "ALL"
    public string Range { get; set; } = "day"; // day, week, month, year
    public List<string> Labels { get; set; } = new(); // e.g. ["Mon", "Tue"]
    public List<double> Current { get; set; } = new();
    public List<double> Previous { get; set; } = new();
}
