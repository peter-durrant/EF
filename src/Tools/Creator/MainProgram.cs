using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hdd.EfData;
using Hdd.EfData.Model;

namespace Hdd.Creator
{
    internal class MainProgram
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Creating database");

            const string databasePath = @"..\..\..\..\database.db";

            if (File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }

            var databaseManager = new DatabaseManager();
            databaseManager.CreateDatabase(databasePath);

            var timestamp = new DateTime(2018, 1, 1, 13, 45, 12);

            var programs = new List<Program>();
            var featureId = 1;
            var measurementId = 1;
            using (var context = new DatabaseContext(databasePath))
            {
                var random = new Random();
                for (var i = 1; i <= 100; i++)
                {
                    var features = new List<Feature>();
                    var featureCount = random.Next(1, 10);
                    for (var featureIndex = 0; featureIndex < featureCount; featureIndex++)
                    {
                        var measurements = new List<Measurement>();
                        var measurementCount = random.Next(1, 20);
                        for (var measurementIndex = 0; measurementIndex < measurementCount; measurementIndex++)
                        {
                            var value = 200.0 * random.NextDouble();
                            var measurement = new Measurement
                            {
                                Id = measurementId,
                                MeasurementType = random.Next(0, 40),
                                Timestamp = timestamp,
                                Instance = 0,
                                Actual = value,
                                Nominal = value + 0.01 * random.NextDouble() * value
                            };
                            measurements.Add(measurement);

                            measurementId++;
                            timestamp += TimeSpan.FromSeconds(30.0 * random.NextDouble());
                        }

                        var feature = new Feature
                        {
                            Id = featureId,
                            FeatureType = random.Next(100, 130),
                            Timestamp = measurements.Last().Timestamp,
                            Status = (Status)random.Next((int)Status.Pass, (int)Status.Error + 1),
                            Measurements = measurements
                        };
                        features.Add(feature);

                        featureId++;
                        timestamp += TimeSpan.FromSeconds(60.0 * random.NextDouble());
                    }

                    var program = new Program
                    {
                        Id = i,
                        ProgramType = random.Next(0, 100),
                        Timestamp = features.Last().Timestamp,
                        Status = (Status)random.Next((int)Status.Pass, (int)Status.Error + 1),
                        Features = features
                    };

                    timestamp += TimeSpan.FromHours(24.0 * random.NextDouble());

                    programs.Add(program);
                }

                context.Programs.AddRange(programs);
                context.SaveChanges();
            }
        }
    }
}
