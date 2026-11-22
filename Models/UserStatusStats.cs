namespace MDMS_PRoject_Backend.Models;

public class UserStatusStats
{
    public string Id { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Group { get; set; } = string.Empty; // admin, consumer
    public int ActiveCount { get; set; }
    public int DeactivatedCount { get; set; }
}
