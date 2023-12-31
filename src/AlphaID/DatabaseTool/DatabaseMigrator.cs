﻿namespace DatabaseTool;
/// <summary>
/// 表示一个数据库迁移器。
/// </summary>
internal abstract class DatabaseMigrator
{
    public abstract Task DropDatabaseAsync();

    public abstract Task MigrateAsync();

    public abstract Task PostMigrationAsync();

    public abstract Task AddTestingDataAsync();
}
