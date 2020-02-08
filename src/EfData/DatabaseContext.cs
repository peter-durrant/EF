using System;
using Hdd.EfData.Model;
using Microsoft.EntityFrameworkCore;

namespace Hdd.EfData
{
    public class DatabaseContext : DbContext
    {
        private readonly string _path;

        public DatabaseContext(string path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public DbSet<Program> Programs { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<BoundedMeasurement> BoundedMeasurements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Program>().HasKey(program => program.Id);
            modelBuilder.Entity<Program>().Property(program => program.Id).IsRequired();
            modelBuilder.Entity<Program>().Property(program => program.ProgramType).IsRequired();
            modelBuilder.Entity<Program>().Property(program => program.Timestamp).IsRequired();
            modelBuilder.Entity<Program>().Property(program => program.Status).IsRequired();

            modelBuilder.Entity<Feature>().HasKey(feature => feature.Id);
            modelBuilder.Entity<Feature>().Property(feature => feature.Id).IsRequired();
            modelBuilder.Entity<Feature>().Property(feature => feature.FeatureType).IsRequired();
            modelBuilder.Entity<Feature>().Property(feature => feature.Timestamp).IsRequired();
            modelBuilder.Entity<Feature>().Property(feature => feature.Status).IsRequired();

            modelBuilder.Entity<Measurement>().HasKey(measurement => measurement.Id);
            modelBuilder.Entity<Measurement>().Property(measurement => measurement.Id).IsRequired();
            modelBuilder.Entity<Measurement>().Property(measurement => measurement.MeasurementType).IsRequired();
            modelBuilder.Entity<Measurement>().Property(measurement => measurement.Timestamp).IsRequired();
            modelBuilder.Entity<Measurement>().Property(measurement => measurement.Instance).IsRequired();
            modelBuilder.Entity<Measurement>().Property(measurement => measurement.Actual).IsRequired();
            modelBuilder.Entity<Measurement>().Property(measurement => measurement.Nominal).IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_path}");
        }
    }
}
