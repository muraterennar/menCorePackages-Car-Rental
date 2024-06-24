using System.Reflection;
using MenCore.Security.Entities;
using Microsoft.EntityFrameworkCore;
using RentACar.Domaim.Entities;

namespace RentACar.Persistence.Contexts;

public class BaseDatabaseContext : DbContext
{
    public BaseDatabaseContext()
    {
    }

    public BaseDatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Fuel> Fuels { get; set; }
    public DbSet<Transmission> Transmissions { get; set; }

    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserLogin> UserLogins { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<OtpAuthenticator> OtpAuthenticators { get; set; }
    public DbSet<EmailAuthenticator> EmailAuthenticators { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            //TODO: Transaction İşleminde Sorun Çıkarıyor => EnableRetryOnFailure()
            //optionsBuilder.UseSqlServer("Server=localhost;Database=TestCarRentalDb;user id=SA;password=MyPass@word;TrustServerCertificate=true", options => options.EnableRetryOnFailure());
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=TestCarRentalDb;user id=SA;password=MyPass@word;TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}