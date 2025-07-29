using GreatOptionTrader.Models;
using IBApi;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using GreatOptionTrader.Abstractions;
using GreatOptionTrader.Services.Converters;

namespace GreatOptionTrader.Services.Connectors;

internal class IbWrapper (
        List<IPriceable<double>> instrumentCache,
        ILogger<InteractiveBroker> logger) : DefaultEWrapper {

    public string[]? Accounts;
    public int ValidOrderId;

    public event ItemRequestedEventHandler<Instrument> OnContractDetailsRequested = delegate { };

    public override void error (int id, int errorCode, string errorMsg, string advancedOrderRejectJson) {
        logger.LogError("{id}: {errorCode}: {message}", id, errorCode, errorMsg);
    }

    public override void error (string str) => logger.LogCritical("{message}", str);
    
    public override void error (Exception e) => logger.LogCritical("{message}", e.Message);
    
    public override void contractDetails (int reqId, ContractDetails contractDetails) {
        OnContractDetailsRequested.Invoke(reqId, contractDetails.ToInstrument());
    }

    public override void managedAccounts (string accountsList) => Accounts = accountsList.Split(",");

    public override void nextValidId (int orderId) => ValidOrderId = orderId;

    public override void tickPrice (int tickerId, int field, double price, TickAttrib attribs) {
        switch (field) {
            case TickType.ASK:
            case TickType.DELAYED_ASK:
                instrumentCache[tickerId].UpdateAskPrice(price);
                break;
            case TickType.BID:
            case TickType.DELAYED_BID:
                instrumentCache[tickerId].UpdateBidPrice(price);
                break;
            case TickType.LAST:
            case TickType.DELAYED_LAST:
                instrumentCache[tickerId].UpdateLastPrice(price);
                break;
            default:
                break;
        }
    }

    public override void orderStatus (
        int orderId, 
        string status, 
        decimal filled, 
        decimal remaining,
        double avgFillPrice,
        int permId,
        int parentId,
        double lastFillPrice,
        int clientId,
        string whyHeld, 
        double mktCapPrice) {
        
    }

    public override void openOrder (
        int orderId, 
        Contract contract, 
        IBApi.Order order, 
        OrderState orderState) {
        
    }
}