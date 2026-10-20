using System;

namespace Core.Domain;

public class Instrument {
    public Instrument(
        InstrumentRight right,
        InstrumentType type,
        int id,
        string name,
        string symbol,
        string exchange,
        DateTime expirationAt
    ) {
        Right = right;
        Type = type;
        Id = id;
        Name = name;
        Symbol = symbol;
        Exchange = exchange;
        ExpirationAt = expirationAt;
    }

    public InstrumentRight Right { get; }
    public InstrumentType Type { get; }
    public int Id { get; }
    public string Name { get; }
    public string Symbol { get; }
    public string Exchange { get; }
    public DateTime ExpirationAt { get; }
}