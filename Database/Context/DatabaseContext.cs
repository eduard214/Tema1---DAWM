using Database.Entities;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Database.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Order>()
            .Property(o => o.OrderDate)
            .HasColumnType("timestamp");

        modelBuilder.Entity<Customer>()
            .Property(c => c.Name)
            .HasColumnType("text");

        modelBuilder.Entity<Customer>()
            .Property(c => c.Email)
            .HasColumnType("text");

        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property("CreatedAt")
                    .HasColumnType("timestamp");

                modelBuilder.Entity(entityType.ClrType)
                    .Property("ModifiedAt")
                    .HasColumnType("timestamp");

                modelBuilder.Entity(entityType.ClrType)
                    .Property("DeletedAt")
                    .HasColumnType("timestamp");
            }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        var connectionString = AppConfig.ConnectionStrings?.DefaultConnection;

        if (connectionString != null)
            optionsBuilder.UseNpgsql(
                connectionString,
                o => o.SetPostgresVersion(16, 0)
            );
    }
}