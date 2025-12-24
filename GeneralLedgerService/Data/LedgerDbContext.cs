using GeneralLedgerService.Domain;
using Microsoft.EntityFrameworkCore;

namespace GeneralLedgerService.Data;

public class LedgerDbContext : DbContext
{
    public LedgerDbContext(DbContextOptions<LedgerDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<JournalEntry> JournalEntries { get; set; } = null!;
    public DbSet<JournalEntryLine> JournalEntryLines { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Balance).HasPrecision(19, 4);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.HasMany(e => e.Lines)
                  .WithOne(e => e.JournalEntry)
                  .HasForeignKey(e => e.JournalEntryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<JournalEntryLine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(19, 4);
            entity.HasOne(e => e.Account)
                  .WithMany()
                  .HasForeignKey(e => e.AccountId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

public static class JournalEntryLineExtensions
{
    // Adding shadow property Id for JournalEntry relationship if not explicitly in domain model
    // But I will add it to the Domain model for simplicity and better mapping
}
