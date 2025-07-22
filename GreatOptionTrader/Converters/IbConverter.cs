using GreatOptionTrader.Models;
using IBApi;
using System;
using System.Globalization;

namespace GreatOptionTrader.Converters;
internal static class IbConverter {
    public static DateTime ToDateTime (this string ibDateTime) =>
        DateTime.ParseExact(ibDateTime, "yyyyMMdd", CultureInfo.InvariantCulture);

    public static Instrument ToInstrument (this ContractDetails contract) {
        return new Instrument() {
            Id = contract.Contract.ConId,
            Name = contract.Contract.LocalSymbol,
            Symbol = contract.Contract.Symbol,
            Exchange = contract.Contract.Exchange,
            Multiplier = int.Parse(contract.Contract.Multiplier),
            ExpirationDate = contract.Contract.LastTradeDate.ToDateTime(),
            Strike = contract.Contract.Strike
        };
    }

    public static Contract ToIbContract(this Instrument instrument) {
        return new Contract() {
            ConId = instrument.Id,
            LocalSymbol = instrument.Name,
            Exchange = instrument.Exchange
        };
    }
}
