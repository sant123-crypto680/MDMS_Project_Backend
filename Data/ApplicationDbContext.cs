using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Models;

namespace MDMS_PRoject_Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Meter> Meters => Set<Meter>();
    public DbSet<Tariff> Tariffs => Set<Tariff>();
    public DbSet<TariffSlab> TariffSlabs => Set<TariffSlab>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<BillDay> BillDays => Set<BillDay>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<MeterReading> MeterReadings => Set<MeterReading>();
    public DbSet<ConsumptionSeries> ConsumptionSeries => Set<ConsumptionSeries>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<ZoneSettings> ZoneSettings => Set<ZoneSettings>();
    public DbSet<EnterpriseSettings> EnterpriseSettings => Set<EnterpriseSettings>();
    public DbSet<UserStatusStats> UserStatusStats => Set<UserStatusStats>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Zone - User (Admin) relationship (1:1 / One zone has one admin)
        modelBuilder.Entity<Zone>(entity =>
        {
            entity.HasKey(z => z.Id);
            entity.HasOne(z => z.Admin)
                  .WithOne(u => u.AdministeredZone)
                  .HasForeignKey<Zone>(z => z.AdminId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // 2. User - Zone relationship (Many users belong to one zone)
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasOne(u => u.Zone)
                  .WithMany(z => z.Users)
                  .HasForeignKey(u => u.ZoneId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // 3. Meter relationships
        modelBuilder.Entity<Meter>(entity =>
        {
            entity.HasKey(m => m.Id);

            // Owner (User)
            entity.HasOne(m => m.Owner)
                  .WithMany(u => u.Meters)
                  .HasForeignKey(m => m.OwnerId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Zone
            entity.HasOne(m => m.Zone)
                  .WithMany(z => z.Meters)
                  .HasForeignKey(m => m.ZoneId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Tariff
            entity.HasOne(m => m.Tariff)
                  .WithMany(t => t.Meters)
                  .HasForeignKey(m => m.TariffId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // 4. Tariff & TariffSlabs
        modelBuilder.Entity<Tariff>(entity =>
        {
            entity.HasKey(t => t.Id);
        });

        modelBuilder.Entity<TariffSlab>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasOne(s => s.Tariff)
                  .WithMany(t => t.Slabs)
                  .HasForeignKey(s => s.TariffId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 5. Bill relationships
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(b => b.Id);

            // User
            entity.HasOne(b => b.User)
                  .WithMany(u => u.Bills)
                  .HasForeignKey(b => b.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Meter
            entity.HasOne(b => b.Meter)
                  .WithMany(m => m.Bills)
                  .HasForeignKey(b => b.MeterId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Zone
            entity.HasOne(b => b.Zone)
                  .WithMany(z => z.Bills)
                  .HasForeignKey(b => b.ZoneId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // 6. BillDay relationships
        modelBuilder.Entity<BillDay>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.HasOne(d => d.Bill)
                  .WithMany(b => b.BillDays)
                  .HasForeignKey(d => d.BillId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 7. Payment relationships
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.Id);

            // Bill
            entity.HasOne(p => p.Bill)
                  .WithMany(b => b.Payments)
                  .HasForeignKey(p => p.BillId)
                  .OnDelete(DeleteBehavior.Restrict);

            // User
            entity.HasOne(p => p.User)
                  .WithMany(u => u.Payments)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // 8. MeterReading relationships
        modelBuilder.Entity<MeterReading>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasOne(r => r.Meter)
                  .WithMany(m => m.Readings)
                  .HasForeignKey(r => r.MeterId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 9. Alert relationships
        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(a => a.Id);

            // Zone
            entity.HasOne(a => a.Zone)
                  .WithMany(z => z.Alerts)
                  .HasForeignKey(a => a.ZoneId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Meter (optional)
            entity.HasOne(a => a.Meter)
                  .WithMany(m => m.Alerts)
                  .HasForeignKey(a => a.MeterId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // 10. Notification relationships
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.HasOne(n => n.User)
                  .WithMany(u => u.Notifications)
                  .HasForeignKey(n => n.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 11. AuditLog relationships
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.HasOne(l => l.User)
                  .WithMany(u => u.AuditLogs)
                  .HasForeignKey(l => l.UserId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // 12. ZoneSettings relationship (1:1 with Zone)
        modelBuilder.Entity<ZoneSettings>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasOne(s => s.Zone)
                  .WithOne(z => z.Settings)
                  .HasForeignKey<ZoneSettings>(s => s.ZoneId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Standalone tables
        modelBuilder.Entity<ConsumptionSeries>().HasKey(c => c.Id);
        modelBuilder.Entity<EnterpriseSettings>().HasKey(e => e.Id);
        modelBuilder.Entity<UserStatusStats>().HasKey(u => u.Id);
    }
}
