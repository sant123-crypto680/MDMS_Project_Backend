using Microsoft.EntityFrameworkCore;

namespace MDMS_PRoject_Backend.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Apply pending EF Core migrations automatically
        context.Database.Migrate();

        // Check if data already exists
        if (context.Zones.Any())
        {
            return; // Already seeded
        }

        // Delegate entire dynamic algorithmic generation to DataGenerator
        DataGenerator.SeedAll(context);
    }

    public static void ResetAndSeed(ApplicationDbContext context)
    {
        // Clear child-to-parent tables in foreign key order
        context.BillDays.ExecuteDelete();
        context.Payments.ExecuteDelete();
        context.Bills.ExecuteDelete();
        context.MeterReadings.ExecuteDelete();
        context.Alerts.ExecuteDelete();
        context.Notifications.ExecuteDelete();
        context.AuditLogs.ExecuteDelete();
        context.ZoneSettings.ExecuteDelete();
        context.Meters.ExecuteDelete();
        context.TariffSlabs.ExecuteDelete();
        context.Tariffs.ExecuteDelete();

        // Break circular Zone-User link before deleting
        foreach (var z in context.Zones.ToList())
        {
            z.AdminId = null;
        }
        context.SaveChanges();

        context.Users.ExecuteDelete();
        context.Zones.ExecuteDelete();
        context.ConsumptionSeries.ExecuteDelete();
        context.EnterpriseSettings.ExecuteDelete();
        context.UserStatusStats.ExecuteDelete();

        // Clear EF Core change tracker memory so new instances can be tracked without conflict
        context.ChangeTracker.Clear();

        // Re-seed cleanly
        DataGenerator.SeedAll(context);
    }
}
