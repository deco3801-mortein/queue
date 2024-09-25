using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace plantMetricHandler.Models;

public partial class MorteinContext : DbContext
{
    public MorteinContext()
    {
    }

    public MorteinContext(DbContextOptions<MorteinContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<PlantDatum> PlantData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? host;
        string? port;
        string? user;
        string? password;
        string? dbName;

        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME")))
        {
            // Load from .env file if not in lambda
            Env.Load();
            host = Env.GetString("POSTGRES_HOST");
            port = Env.GetString("POSTGRES_PORT");
            user = Env.GetString("POSTGRES_USER");
            password = Env.GetString("POSTGRES_PASSWORD");
            dbName = Env.GetString("POSTGRES_DB");
        }
        else
        {
            host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            user = Environment.GetEnvironmentVariable("POSTGRES_USER");
            password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
        }

        string connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={dbName};";
        optionsBuilder.UseNpgsql(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("Device");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<PlantDatum>(entity =>
        {
            entity.ToTable("plant_data");
            entity.Property(e => e.SensorId)
                .IsRequired()
                .HasColumnName("sensor_id");
            entity.HasKey(e => e.SensorId);
            entity.Property(e => e.Moisture).HasColumnName("moisture");
            entity.Property(e => e.Sunlight).HasColumnName("sunlight");
            entity.Property(e => e.Temp).HasColumnName("temp");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            entity.Property(e => e.VibrationStatus).HasColumnName("vibration_status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
