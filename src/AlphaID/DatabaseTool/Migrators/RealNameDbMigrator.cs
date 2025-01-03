﻿using System.Text;
using AlphaId.RealName.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DatabaseTool.Migrators;

internal class RealNameDbMigrator(RealNameDbContext db) : DatabaseMigrator(db)
{
    public override async Task AddTestingDataAsync()
    {
        string[] idSubjectsDbSqlFiles = Directory.GetFiles("./TestingData/RealNameDbContext", "*.sql");
        foreach (string file in idSubjectsDbSqlFiles)
            await db.Database.ExecuteSqlRawAsync(await File.ReadAllTextAsync(file, Encoding.UTF8));
    }
}