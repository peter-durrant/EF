using System;

namespace Hdd.Creator
{
    internal class MainProgram
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Creating database");

            const string databasePath = @"..\..\..\..\database.db";

            var databaseCreator = new DatabaseCreator();
            databaseCreator.Create(databasePath);
        }
    }
}
