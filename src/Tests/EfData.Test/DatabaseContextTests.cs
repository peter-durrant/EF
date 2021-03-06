﻿using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Hdd.EfData.Test
{
    [TestFixture]
    public class DatabaseContextTests
    {
        private readonly string _databasePath = $@"{TestContext.CurrentContext.TestDirectory}\TestData\database.db";

        [Test]
        public void DatabaseContext_TestDataExists()
        {
            Assert.IsTrue(File.Exists(_databasePath));
        }

        [Test]
        public void DatabaseContext_Ctor_DatabaseProviderName_IsSqlite()
        {
            using (var context = new DatabaseContext(_databasePath))
            {
                Assert.AreEqual("Microsoft.EntityFrameworkCore.Sqlite", context.Database.ProviderName);
            }
        }

        [Test]
        public void DatabaseContext_Programs_HasExpectedCount()
        {
            using (var context = new DatabaseContext(_databasePath))
            {
                Assert.AreEqual(100, context.Programs.Count());
            }
        }

        [Test]
        public void DatabaseContext_Features_HasExpectedCount()
        {
            using (var context = new DatabaseContext(_databasePath))
            {
                Assert.AreEqual(544, context.Features.Count());
            }
        }

        [Test]
        public void DatabaseContext_Measurements_HasExpectedCount()
        {
            using (var context = new DatabaseContext(_databasePath))
            {
                Assert.AreEqual(9014, context.Measurements.Count());
            }
        }

        [Test]
        public void DatabaseContext_Measurements_NotBoundedMeasurements_HasExpectedCount()
        {
            using (var context = new DatabaseContext(_databasePath))
            {
                Assert.AreEqual(1522, context.Measurements.Count() - context.BoundedMeasurements.Count());
            }
        }

        [Test]
        public void DatabaseContext_BoundedMeasurements_HasExpectedCount()
        {
            using (var context = new DatabaseContext(_databasePath))
            {
                Assert.AreEqual(7492, context.BoundedMeasurements.Count());
            }
        }
    }
}
