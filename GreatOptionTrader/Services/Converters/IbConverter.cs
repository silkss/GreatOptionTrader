using Core;
using GreatOptionTrader.Models;
using IBApi;
using System;
using System.Globalization;
using System.Linq;

namespace GreatOptionTrader.Services.Converters;

internal static class IbConverter {
    public static DateTime ToDateTime (this string ibDateTime) =>
        DateTime.ParseExact(ibDateTime, "yyyyMMdd", CultureInfo.InvariantCulture);

    public static OptionModel ToInstrument (this ContractDetails contract) {
        return new OptionModel() {
            Id = contract.Contract.ConId,
            Name = contract.Contract.LocalSymbol,
            Symbol = contract.Contract.Symbol,
            Exchange = contract.Contract.Exchange,
            Multiplier = int.Parse(contract.Contract.Multiplier),
            ExpirationDate = contract.Contract.LastTradeDate.ToDateTime(),
            Strike = (decimal)contract.Contract.Strike,
            PriceMagnifier = contract.PriceMagnifier,
            Right = contract.Contract.Right == "PUT" ? OptionRight.Put : OptionRight.Call,
            TradingClass = contract.Contract.TradingClass,
            MarketRulesId = [.. contract.MarketRuleIds.Split(",").Distinct().Select(int.Parse)]
        };
    }

    public static Contract ToIBContract (this OptionModel instrument) => new() {
        ConId = instrument.Id,
        Exchange = instrument.Exchange,
        LastTradeDateOrContractMonth = instrument.ExpirationDate.ToString("yyyyMMdd")
    };

    public static IBApi.Order ToIBLimitOrder (this Core.Order order) => new() {
        Account = order.Account,
        OrderId = order.OrderId,
        TotalQuantity = order.Quantity,
        Action = order.Direction == TradeDirection.Buy ? "BUY" : "SELL",
        LmtPrice = (double)order.LimitPrice,
        OrderType = "LMT",
        Tif = "GTC"
    };

    public static OrderStatus ToOrderStatus (this string status) => status switch {
        "PendingSubmit" => OrderStatus.PendingSubmit,
        "PendingCancel" => OrderStatus.PendingSubmit,
        "PreSubmitted" => OrderStatus.PreSubmitted,
        "Submitted" => OrderStatus.Submitted,
        "ApiCancelled" => OrderStatus.ApiCancelled,
        "Cancelled" => OrderStatus.Cancelled,
        "Filled" => OrderStatus.Filled,
        "Inactive" => OrderStatus.Inactive,
        _ => throw new NotSupportedException($"Unsupported order status {status}")
    };
}
