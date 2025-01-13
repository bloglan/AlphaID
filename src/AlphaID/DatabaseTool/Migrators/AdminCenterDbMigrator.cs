using System.Text;
using AdminWebApp.Infrastructure.DataStores;
using Microsoft.EntityFrameworkCore;

namespace DatabaseTool.Migrators;

internal class AdminCenterDbMigrator(OperationalDbContext db) : DatabaseMigrator(db)
{
    public override async Task AddTestingDataAsync()
    {
        string[] files = Directory.GetFiles("./TestingData/AdminWebAppData", "*.sql");
        foreach (string file in files)
            await db.Database.ExecuteSqlRawAsync(await File.ReadAllTextAsync(file, Encoding.UTF8));
    }
}