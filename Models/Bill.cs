using System.Text.Json.Serialization;

namespace MDMS_PRoject_Backend.Models;

public class Bill
{
    public string Id { get; set; } = string.Empty; // e.g. BILL-2026-10-USR001
    public string UserId { get; set; } = string.Empty; // FK to User
    public string MeterId { get; set; } = string.Empty; // FK to Meter
    public string ZoneId { get; set; } = string.Empty; // FK to Zone
    public string BillingMonth { get; set; } = string.Empty; // e.g. 2026-10
    public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public double PreviousReading { get; set; }
    public double CurrentReading { get; set; }
    public double TotalUnits { get; set; } // kWh
    public decimal EnergyCharge { get; set; } // INR
    public decimal FixedCharge { get; set; } // INR
    public decimal TaxAmount { get; set; } // INR
    public decimal Amount { get; set; } // Total INR
    public string Status { get; set; } = "Pending"; // Paid, Pending, Overdue
    public DateTime? PaidOn { get; set; }

    // Navigation Properties
    public User? User { get; set; }
    public Meter? Meter { get; set; }
    public Zone? Zone { get; set; }

    public ICollection<BillDay> BillDays { get; set; } = new List<BillDay>();

    [JsonIgnore]
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
