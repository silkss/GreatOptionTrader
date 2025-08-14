using Microsoft.EntityFrameworkCore;
using Core;
using System;

namespace GreatOptionTrader.Models;
public class GOTContext : DbContext {
    public DbSet<InstrumentGroup>? InstrumentGroups { get; set; }
    public DbSet<OptionModel>? Options { get; set; }
    public DbSet<Order>? Orders { get; set; }

    public string DbPath { get; }

    public GOTContext () {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "GOT.db");
    }

    protected override void OnConfiguring (DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating (ModelBuilder modelBuilder) {
        modelBuilder.Entity<InstrumentGroup>()
            .HasMany(e => e.Options)
            .WithOne()
            .HasForeignKey(e => e.InstrumentGroupId)
            .IsRequired(true);

        modelBuilder.Entity<OptionModel>()
            .HasMany(e => e.Orders)
            .WithOne()
            .HasForeignKey(e => e.InstrumentId)
            .IsRequired(true);
    }
}
