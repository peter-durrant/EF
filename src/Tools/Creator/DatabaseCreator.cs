using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hdd.EfData;
using Hdd.EfData.Model;

namespace Hdd.Creator
{
    public class DatabaseCreator
    {
        private const double BoundedMeasurementWarningTolerance = 0.8;
        private readonly Random random = new Random();
        private int measurementInstance = 1;
        private int featureId = 1;
        private int measurementId = 1;
        private DateTime timestamp;

        public void Create(string databasePath)
        {
            if (File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }

            var databaseManager = new DatabaseManager();
            databaseManager.CreateDatabase(databasePath);

            timestamp = new DateTime(2018, 1, 1, 13, 45, 12);

            var programs = new List<Program>();

            using (var context = new DatabaseContext(databasePath))
            {
                for (var programId = 1; programId <= 100; programId++)
                {
                    var features = GenerateFeatures();

                    var program = new Program
                    {
                        Id = programId,
                        ProgramType = random.Next(0, 100),
                        Timestamp = features.Last().Timestamp,
                        Status = GetProgramStatusFromFeatures(features),
                        Features = features
                    };

                    programs.Add(program);
                }

                context.Programs.AddRange(programs);
                context.SaveChanges();
            }
        }

        private List<Feature> GenerateFeatures()
        {
            var features = new List<Feature>();

            var featuresPerProgram = random.Next(1, 10);
            for (var i = 0; i < featuresPerProgram; i++)
            {
                var measurements = GenerateMeasurements(out var featureStatus);
                measurementId = measurements.Last().Id + 1;

                var feature = new Feature
                {
                    Id = featureId,
                    FeatureType = random.Next(100, 130),
                    Timestamp = measurements.Last().Timestamp,
                    Status = featureStatus,
                    Measurements = measurements
                };

                features.Add(feature);
                featureId++;
                timestamp = feature.Timestamp + TimeSpan.FromSeconds(60.0 * random.NextDouble());
            }

            return features;
        }

        private List<Measurement> GenerateMeasurements(out Status featureStatus)
        {
            featureStatus = Status.None;

            var measurements = new List<Measurement>();

            var measurementsPerFeature = random.Next(3, 20);
            for (var j = 0; j < measurementsPerFeature; j++)
            {
                var nominal = 200.0 * random.NextDouble();
                var actual = nominal + 0.1 * (random.NextDouble() - 0.5) * nominal;

                var isBoundedMeasurementType = random.Next() % 4 != 0;

                if (!isBoundedMeasurementType)
                {
                    var measurement = new Measurement
                    {
                        Id = measurementId,
                        Instance = measurementInstance,
                        MeasurementType = random.Next(0, 40),
                        Timestamp = timestamp,
                        RemeasureInstance = 0,
                        Actual = actual,
                        Nominal = nominal
                    };

                    timestamp += TimeSpan.FromSeconds(30.0 * random.NextDouble());
                    measurements.Add(measurement);
                    measurementId++;
                }
                else
                {
                    var remeasureInstance = 0;

                    const double range = 0.03;
                    var lower = (1.0 - range) * nominal;
                    var upper = (1.0 + range) * nominal;
                    var lowerWarn = (1.0 - range * BoundedMeasurementWarningTolerance) * nominal;
                    var upperWarn = (1.0 + range * BoundedMeasurementWarningTolerance) * nominal;
                    var status = Status.Pass;
                    if (actual < lower || actual > upper)
                    {
                        status = Status.Fail;
                    }
                    else if (actual < lowerWarn || actual > upperWarn)
                    {
                        status = Status.Warn;
                    }

                    var measurement = new BoundedMeasurement
                    {
                        Id = measurementId,
                        Instance = measurementInstance,
                        MeasurementType = random.Next(0, 40),
                        Timestamp = timestamp,
                        Status = status,
                        RemeasureInstance = remeasureInstance,
                        Actual = actual,
                        Nominal = nominal,
                        Lower = lower,
                        Upper = upper
                    };

                    timestamp += TimeSpan.FromSeconds(30.0 * random.NextDouble());
                    measurements.Add(measurement);
                    measurementId++;

                    if (status != Status.Pass)
                    {
                        var remeasuresPerMeasurement = random.Next(1, 5);
                        for (var k = 0; k < remeasuresPerMeasurement; k++)
                        {
                            remeasureInstance++;

                            var actualRemeasure = nominal + 0.07 * (random.NextDouble() - 0.5) * nominal;

                            status = Status.Pass;
                            if (actualRemeasure < lower || actualRemeasure > upper)
                            {
                                status = Status.Fail;
                            }
                            else if (actualRemeasure < lowerWarn || actualRemeasure > upperWarn)
                            {
                                status = Status.Warn;
                            }

                            var remeasure = new BoundedMeasurement
                            {
                                Id = measurementId,
                                Instance = measurementInstance,
                                MeasurementType = measurement.MeasurementType,
                                Timestamp = timestamp,
                                Status = status,
                                RemeasureInstance = remeasureInstance,
                                Actual = actualRemeasure,
                                Nominal = nominal,
                                Lower = lower,
                                Upper = upper
                            };

                            timestamp += TimeSpan.FromSeconds(30.0 * random.NextDouble());
                            measurements.Add(remeasure);
                            measurementId++;

                            if (status == Status.Pass)
                            {
                                break;
                            }
                        }
                    }

                    if (measurements.OfType<BoundedMeasurement>().Last().Status > featureStatus)
                    {
                        featureStatus = status;
                    }
                }

                measurementInstance++;
            }

            return measurements;
        }

        private static Status GetProgramStatusFromFeatures(IReadOnlyCollection<Feature> features)
        {
            return features.Any() ? features.Max(feature => feature.Status) : Status.None;
        }
    }
}
