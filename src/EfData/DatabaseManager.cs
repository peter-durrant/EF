using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hdd.EfData
{
    public class DatabaseManager
    {
        public void DeleteDatabase(string path)
        {
            using (var context = new DatabaseContext(path))
            {
                context.Database.EnsureDeleted();
            }
        }

        public void CreateDatabase(string path)
        {
            using (var context = new DatabaseContext(path))
            {
                var databaseCreator = context.GetService<IRelationalDatabaseCreator>();
                if (!databaseCreator.Exists())
                {
                    databaseCreator.CreateTables();
                }
                else
                {
                    context.Database.Migrate();
                }

                context.SaveChanges();
            }
        }
    }
}
