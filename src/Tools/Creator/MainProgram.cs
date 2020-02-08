using System;
using System.IO;
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

            using (var context = new DatabaseContext(databasePath))
            {
                var random = new Random();
                for (var i = 1; i <= 100; i++)
                {
                    var program = new Program
                    {
                        Id = i,
                        ProgramType = random.Next(0, 100),
                        Status = (Status)random.Next((int)Status.Pass, (int)Status.Error + 1)
                    };

                    context.Programs.Add(program);
                }

                context.SaveChanges();
            }
        }
    }
}
