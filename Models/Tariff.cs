using System.Text.Json.Serialization;

namespace MDMS_PRoject_Backend.Models;

public class Tariff
{
    public string Id { get; set; } = string.Empty; // e.g. TRF-RES
    public string Category { get; set; } = string.Empty; // Residential, Commercial, Industrial
    public decimal FixedCharge { get; set; } // INR per month
    public decimal TaxPercent { get; set; }
    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<TariffSlab> Slabs { get; set; } = new List<TariffSlab>();

    [JsonIgnore]
    public ICollection<Meter> Meters { get; set; } = new List<Meter>();
}
