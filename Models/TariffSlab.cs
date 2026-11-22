namespace MDMS_PRoject_Backend.Models;

public class TariffSlab
{
    public string Id { get; set; } = string.Empty;
    public string TariffId { get; set; } = string.Empty; // FK to Tariff
    public decimal FromUnit { get; set; }
    public decimal? ToUnit { get; set; } // null for the last slab
    public decimal RatePerUnit { get; set; } // INR per kWh

    // Navigation Properties
    public Tariff? Tariff { get; set; }
}
