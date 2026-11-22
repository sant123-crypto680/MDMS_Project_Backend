namespace MDMS_PRoject_Backend.Models;

public class Payment
{
    public string Id { get; set; } = string.Empty;
    public string BillId { get; set; } = string.Empty; // FK to Bill
    public string UserId { get; set; } = string.Empty; // FK to User
    public decimal AmountPaid { get; set; } // INR
    public string Method { get; set; } = "UPI"; // UPI, Card, NetBanking
    public string? TransactionRef { get; set; }
    public string Status { get; set; } = "Success"; // Success, Failed
    public DateTime PaidOn { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Bill? Bill { get; set; }
    public User? User { get; set; }
}
