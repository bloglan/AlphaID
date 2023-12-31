﻿using IdSubjects.DirectoryLogon;
using Microsoft.EntityFrameworkCore;

namespace AlphaId.DirectoryLogon.EntityFramework;

public class DirectoryLogonDbContext : DbContext
{
    public DirectoryLogonDbContext(DbContextOptions<DirectoryLogonDbContext> options) : base(options)
    {
    }

    public DbSet<DirectoryService> DirectoryServices { get; protected set; } = default!;

    public DbSet<LogonAccount> LogonAccounts { get; protected set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogonAccount>().Property(p => p.LogonId).UseCollation("Chinese_PRC_CS_AS");
    }
}
