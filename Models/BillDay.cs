namespace MDMS_PRoject_Backend.Models;

public class BillDay
{
    public string Id { get; set; } = string.Empty;
    public string BillId { get; set; } = string.Empty; // FK to Bill
    public DateTime Date { get; set; }
    public double Reading { get; set; }
    public double Consumption { get; set; } // kWh
    public decimal Cost { get; set; } // INR

    // Navigation Properties
    public Bill? Bill { get; set; }
}
