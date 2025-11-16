using Microsoft.EntityFrameworkCore;
using Payments.Domain;

namespace Payments.Infra;

public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<PlannedBalance> PlannedBalances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Amount).IsRequired();
            entity.Property(e => e.PaymentDate).HasConversion(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            entity.Property(e => e.Completed).IsRequired();
            entity.Property(e => e.CreatedAt).HasConversion(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            entity.HasOne(e => e.Profile).WithMany().HasForeignKey("ProfileId").IsRequired();
            entity.HasOne(e => e.PlannedBalance).WithMany().HasForeignKey("PlannedBalanceId").IsRequired(false);

            entity.Navigation(e => e.Profile).AutoInclude();
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<PlannedBalance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Year).IsRequired();
            entity.Property(e => e.Month).IsRequired();
            entity.Property(e => e.Amount).IsRequired();
            entity.Property(e => e.Category).IsRequired().HasMaxLength(255);
            entity.HasOne(e => e.Profile).WithMany().HasForeignKey("ProfileId").IsRequired();

            entity.Navigation(e => e.Profile).AutoInclude();
        });
    }
}