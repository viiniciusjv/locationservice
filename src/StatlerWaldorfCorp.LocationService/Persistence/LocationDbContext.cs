using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using StatlerWaldorfCorp.LocationService.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore.Design;
using System;
using Microsoft.Extensions.Configuration;

namespace StatlerWaldorfCorp.LocationService.Persistence
{
    public class LocationDbContext : DbContext
    {
        public LocationDbContext(DbContextOptions<LocationDbContext> options) :base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
        }

        public DbSet<LocationRecord> LocationRecords {get; set;}
    }

    public class LocationDbContextFactory : IDesignTimeDbContextFactory<LocationDbContext>
    {       
        public LocationDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(System.IO.Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true)
             .AddEnvironmentVariables()
             .AddCommandLine(Startup.Args);

            IConfigurationRoot Configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<LocationDbContext>();
         
            var connectionString = Configuration?.GetSection("postgres:cstr")?.Value;
          
            optionsBuilder.UseNpgsql(connectionString);

            return new LocationDbContext(optionsBuilder.Options);
        }
    }
}
