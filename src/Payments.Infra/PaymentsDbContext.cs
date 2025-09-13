using Microsoft.EntityFrameworkCore;
using Payments.Domain;

namespace Payments.Infra;

public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Profile> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Amount).IsRequired();
            entity.Property(e => e.PaymentDate);
            entity.Property(e => e.Completed).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.HasOne(e => e.Profile).WithMany().HasForeignKey("ProfileId").IsRequired();

            entity.Navigation(e => e.Profile).AutoInclude();
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        });
    }
}