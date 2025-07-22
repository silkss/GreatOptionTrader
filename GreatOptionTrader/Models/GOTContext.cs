using Microsoft.EntityFrameworkCore;
using System;

namespace GreatOptionTrader.Models;
public class GOTContext : DbContext {
    public DbSet<InstrumentGroup>? InstrumentGroups { get; set; }
    public DbSet<Instrument>? Instruments { get; set; }

    public string DbPath { get; }

    public GOTContext () {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "GOT.db");
    }

    protected override void OnConfiguring (DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
