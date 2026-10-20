using IBApi;
using IBApi.protobuf;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Connectors.InteractiveBrokers;

partial class Wrapper (ILogger<Connector> logger) : EWrapper {
    private readonly ILogger<Connector> _logger = logger;
    private int _validOrderId;

    public int ValidOrderId => _validOrderId++;
    public string[]? Accounts;

    [LoggerMessage(Level = LogLevel.Error, Message = "{requestId}: {errorCode}: {errorMessage}")]
    partial void logError (int requestId, int errorCode, string errorMessage);

    public void accountDataEndProtoBuf (AccountDataEnd accountDataEndProto) {
        throw new NotImplementedException();
    }

    public void accountDownloadEnd (string account) {
        throw new NotImplementedException();
    }

    public void accountSummary (int reqId, string account, string tag, string value, string currency) {
        throw new NotImplementedException();
    }

    public void accountSummaryEnd (int reqId) {
        throw new NotImplementedException();
    }

    public void accountSummaryEndProtoBuf (AccountSummaryEnd accountSummaryEndProto) {
        throw new NotImplementedException();
    }

    public void accountSummaryProtoBuf (AccountSummary accountSummaryProto) {
        throw new NotImplementedException();
    }

    public void accountUpdateMulti (int requestId, string account, string modelCode, string key, string value, string currency) {
        throw new NotImplementedException();
    }

    public void accountUpdateMultiEnd (int requestId) {
        throw new NotImplementedException();
    }

    public void accountUpdateMultiEndProtoBuf (AccountUpdateMultiEnd accountUpdateMultiEndProto) {
        throw new NotImplementedException();
    }

    public void accountUpdateMultiProtoBuf (AccountUpdateMulti accountUpdateMultiProto) {
        throw new NotImplementedException();
    }

    public void bondContractDataProtoBuf (ContractData contractDataProto) {
        throw new NotImplementedException();
    }

    public void bondContractDetails (int reqId, IBApi.ContractDetails contract) {
        throw new NotImplementedException();
    }

    public void commissionAndFeesReport (IBApi.CommissionAndFeesReport commissionAndFeesReport) {
        throw new NotImplementedException();
    }

    public void commissionAndFeesReportProtoBuf (IBApi.protobuf.CommissionAndFeesReport commissionAndFeesReportProto) {
        throw new NotImplementedException();
    }

    public void completedOrder (IBApi.Contract contract, IBApi.Order order, IBApi.OrderState orderState) {
        throw new NotImplementedException();
    }

    public void completedOrderProtoBuf (CompletedOrder completedOrderProto) {
        throw new NotImplementedException();
    }

    public void completedOrdersEnd () {
        throw new NotImplementedException();
    }

    public void completedOrdersEndProtoBuf (CompletedOrdersEnd completedOrdersEndProto) {
        throw new NotImplementedException();
    }

    public void configResponseProtoBuf (ConfigResponse configResponseProto) {
        throw new NotImplementedException();
    }

    public void connectAck () {
        throw new NotImplementedException();
    }

    public void connectionClosed () {
        throw new NotImplementedException();
    }

    public void contractDataEndProtoBuf (ContractDataEnd contractDataEndProto) {
        throw new NotImplementedException();
    }

    public void contractDataProtoBuf (ContractData contractDataProto) {
        throw new NotImplementedException();
    }

    public void contractDetails (int reqId, IBApi.ContractDetails contractDetails) {
        throw new NotImplementedException();
    }

    public void contractDetailsEnd (int reqId) {
        throw new NotImplementedException();
    }

    public void currentTime (long time) {
        throw new NotImplementedException();
    }

    public void currentTimeInMillis (long timeInMillis) {
        throw new NotImplementedException();
    }

    public void currentTimeInMillisProtoBuf (CurrentTimeInMillis currentTimeInMillisProto) {
        throw new NotImplementedException();
    }

    public void currentTimeProtoBuf (CurrentTime currentTimeProto) {
        throw new NotImplementedException();
    }

    public void deltaNeutralValidation (int reqId, IBApi.DeltaNeutralContract deltaNeutralContract) {
        throw new NotImplementedException();
    }

    public void displayGroupList (int reqId, string groups) {
        throw new NotImplementedException();
    }

    public void displayGroupListProtoBuf (DisplayGroupList displayGroupListProto) {
        throw new NotImplementedException();
    }

    public void displayGroupUpdated (int reqId, string contractInfo) {
        throw new NotImplementedException();
    }

    public void displayGroupUpdatedProtoBuf (DisplayGroupUpdated displayGroupUpdatedProto) {
        throw new NotImplementedException();
    }

    public void error (Exception e) => logError(-1, -1, e.Message);
    public void error (string str) => logError(-1, -1, str);
    public void error (int id, long errorTime, int errorCode, string errorMsg, string advancedOrderRejectJson) {
        logError(id, errorCode, errorMsg);
    }
    public void errorProtoBuf (ErrorMessage errorMessageProto) { }

    public void execDetails (int reqId, IBApi.Contract contract, IBApi.Execution execution) {
        throw new NotImplementedException();
    }

    public void execDetailsEnd (int reqId) {
        throw new NotImplementedException();
    }

    public void execDetailsEndProtoBuf (ExecutionDetailsEnd executionDetailsEndProto) {
        throw new NotImplementedException();
    }

    public void execDetailsProtoBuf (ExecutionDetails executionDetailsProto) {
        throw new NotImplementedException();
    }

    public void familyCodes (IBApi.FamilyCode[] familyCodes) {
        throw new NotImplementedException();
    }

    public void familyCodesProtoBuf (FamilyCodes familyCodesProto) {
        throw new NotImplementedException();
    }

    public void headTimestamp (int reqId, string headTimestamp) {
        throw new NotImplementedException();
    }

    public void headTimestampProtoBuf (HeadTimestamp headTimestampProto) {
        throw new NotImplementedException();
    }

    public void histogramData (int reqId, HistogramEntry[] data) {
        throw new NotImplementedException();
    }

    public void histogramDataProtoBuf (HistogramData histogramDataProto) {
        throw new NotImplementedException();
    }

    public void historicalData (int reqId, Bar bar) {
        throw new NotImplementedException();
    }

    public void historicalDataEnd (int reqId, string start, string end) {
        throw new NotImplementedException();
    }

    public void historicalDataEndProtoBuf (HistoricalDataEnd historicalDataEndProto) {
        throw new NotImplementedException();
    }

    public void historicalDataProtoBuf (HistoricalData historicalDataProto) {
        throw new NotImplementedException();
    }

    public void historicalDataUpdate (int reqId, Bar bar) {
        throw new NotImplementedException();
    }

    public void historicalDataUpdateProtoBuf (HistoricalDataUpdate historicalDataUpdateProto) {
        throw new NotImplementedException();
    }

    public void historicalNews (int requestId, string time, string providerCode, string articleId, string headline) {
        throw new NotImplementedException();
    }

    public void historicalNewsEnd (int requestId, bool hasMore) {
        throw new NotImplementedException();
    }

    public void historicalNewsEndProtoBuf (HistoricalNewsEnd historicalNewsEndProto) {
        throw new NotImplementedException();
    }

    public void historicalNewsProtoBuf (HistoricalNews historicalNewsProto) {
        throw new NotImplementedException();
    }

    public void historicalSchedule (int reqId, string startDateTime, string endDateTime, string timeZone, IBApi.HistoricalSession[] sessions) {
        throw new NotImplementedException();
    }

    public void historicalScheduleProtoBuf (HistoricalSchedule historicalScheduleProto) {
        throw new NotImplementedException();
    }

    public void historicalTicks (int reqId, IBApi.HistoricalTick[] ticks, bool done) {
        throw new NotImplementedException();
    }

    public void historicalTicksBidAsk (int reqId, IBApi.HistoricalTickBidAsk[] ticks, bool done) {
        throw new NotImplementedException();
    }

    public void historicalTicksBidAskProtoBuf (HistoricalTicksBidAsk historicalTicksBidAskProto) {
        throw new NotImplementedException();
    }

    public void historicalTicksLast (int reqId, IBApi.HistoricalTickLast[] ticks, bool done) {
        throw new NotImplementedException();
    }

    public void historicalTicksLastProtoBuf (HistoricalTicksLast historicalTicksLastProto) {
        throw new NotImplementedException();
    }

    public void historicalTicksProtoBuf (HistoricalTicks historicalTicksProto) {
        throw new NotImplementedException();
    }

    public void managedAccounts (string accountsList) => Accounts = accountsList.Split(',').ToArray();
    public void managedAccountsProtoBuf (ManagedAccounts managedAccountsProto) { }

    public void marketDataType (int reqId, int marketDataType) {
        throw new NotImplementedException();
    }

    public void marketDataTypeProtoBuf (MarketDataType marketDataTypeProto) {
        throw new NotImplementedException();
    }

    public void marketDepthExchangesProtoBuf (MarketDepthExchanges marketDepthExchangesProto) {
        throw new NotImplementedException();
    }

    public void marketRule (int marketRuleId, IBApi.PriceIncrement[] priceIncrements) {
        throw new NotImplementedException();
    }

    public void marketRuleProtoBuf (MarketRule marketRuleProto) {
        throw new NotImplementedException();
    }

    public void mktDepthExchanges (DepthMktDataDescription[] depthMktDataDescriptions) {
        throw new NotImplementedException();
    }

    public void newsArticle (int requestId, int articleType, string articleText) {
        throw new NotImplementedException();
    }

    public void newsArticleProtoBuf (NewsArticle newsArticleProto) {
        throw new NotImplementedException();
    }

    public void newsProviders (IBApi.NewsProvider[] newsProviders) {
        throw new NotImplementedException();
    }

    public void newsProvidersProtoBuf (NewsProviders newsProvidersProto) {
        throw new NotImplementedException();
    }

    public void nextValidId (int orderId) => _validOrderId = orderId;
    public void nextValidIdProtoBuf (NextValidId nextValidIdProto) { }

    public void openOrder (int orderId, IBApi.Contract contract, IBApi.Order order, IBApi.OrderState orderState) {
        throw new NotImplementedException();
    }

    public void openOrderEnd () {
        throw new NotImplementedException();
    }

    public void openOrderProtoBuf (OpenOrder openOrderProto) {
        throw new NotImplementedException();
    }

    public void openOrdersEndProtoBuf (OpenOrdersEnd openOrdersEndProto) {
        throw new NotImplementedException();
    }

    public void orderBound (long permId, int clientId, int orderId) {
        throw new NotImplementedException();
    }

    public void orderBoundProtoBuf (OrderBound orderBoundProto) {
        throw new NotImplementedException();
    }

    public void orderStatus (int orderId, string status, decimal filled, decimal remaining, double avgFillPrice, long permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice) {
        throw new NotImplementedException();
    }

    public void orderStatusProtoBuf (IBApi.protobuf.OrderStatus orderStatusProto) {
        throw new NotImplementedException();
    }

    public void pnl (int reqId, double dailyPnL, double unrealizedPnL, double realizedPnL) {
        throw new NotImplementedException();
    }

    public void pnlProtoBuf (PnL pnlProto) {
        throw new NotImplementedException();
    }

    public void pnlSingle (int reqId, decimal pos, double dailyPnL, double unrealizedPnL, double realizedPnL, double value) {
        throw new NotImplementedException();
    }

    public void pnlSingleProtoBuf (PnLSingle pnlSingleProto) {
        throw new NotImplementedException();
    }

    public void position (string account, IBApi.Contract contract, decimal pos, double avgCost) {
        throw new NotImplementedException();
    }

    public void positionEnd () {
        throw new NotImplementedException();
    }

    public void positionEndProtoBuf (PositionEnd positionEndProto) {
        throw new NotImplementedException();
    }

    public void positionMulti (int requestId, string account, string modelCode, IBApi.Contract contract, decimal pos, double avgCost) {
        throw new NotImplementedException();
    }

    public void positionMultiEnd (int requestId) {
        throw new NotImplementedException();
    }

    public void positionMultiEndProtoBuf (PositionMultiEnd positionMultiEndProto) {
        throw new NotImplementedException();
    }

    public void positionMultiProtoBuf (PositionMulti positionMultiProto) {
        throw new NotImplementedException();
    }

    public void positionProtoBuf (Position positionProto) {
        throw new NotImplementedException();
    }

    public void realtimeBar (int reqId, long date, double open, double high, double low, double close, decimal volume, decimal WAP, int count) {
        throw new NotImplementedException();
    }

    public void realTimeBarTickProtoBuf (RealTimeBarTick realTimeBarTickProto) {
        throw new NotImplementedException();
    }

    public void receiveFA (int faDataType, string faXmlData) {
        throw new NotImplementedException();
    }

    public void receiveFAProtoBuf (ReceiveFA receiveFAProto) {
        throw new NotImplementedException();
    }

    public void replaceFAEnd (int reqId, string text) {
        throw new NotImplementedException();
    }

    public void replaceFAEndProtoBuf (ReplaceFAEnd replaceFAEndProto) {
        throw new NotImplementedException();
    }

    public void rerouteMarketDataRequestProtoBuf (RerouteMarketDataRequest rerouteMarketDataRequestProto) {
        throw new NotImplementedException();
    }

    public void rerouteMarketDepthRequestProtoBuf (RerouteMarketDepthRequest rerouteMarketDepthRequestProto) {
        throw new NotImplementedException();
    }

    public void rerouteMktDataReq (int reqId, int conId, string exchange) {
        throw new NotImplementedException();
    }

    public void rerouteMktDepthReq (int reqId, int conId, string exchange) {
        throw new NotImplementedException();
    }

    public void scannerData (int reqId, int rank, IBApi.ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr) {
        throw new NotImplementedException();
    }

    public void scannerDataEnd (int reqId) {
        throw new NotImplementedException();
    }

    public void scannerDataProtoBuf (ScannerData scannerDataProto) {
        throw new NotImplementedException();
    }

    public void scannerParameters (string xml) {
        throw new NotImplementedException();
    }

    public void scannerParametersProtoBuf (ScannerParameters scannerParametersProto) {
        throw new NotImplementedException();
    }

    public void secDefOptParameterEndProtoBuf (SecDefOptParameterEnd secDefOptParameterEndProto) {
        throw new NotImplementedException();
    }

    public void secDefOptParameterProtoBuf (SecDefOptParameter secDefOptParameterProto) {
        throw new NotImplementedException();
    }

    public void securityDefinitionOptionParameter (int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes) {
        throw new NotImplementedException();
    }

    public void securityDefinitionOptionParameterEnd (int reqId) {
        throw new NotImplementedException();
    }

    public void smartComponents (int reqId, Dictionary<int, KeyValuePair<string, char>> theMap) {
        throw new NotImplementedException();
    }

    public void smartComponentsProtoBuf (SmartComponents smartComponentsProto) {
        throw new NotImplementedException();
    }

    public void softDollarTiers (int reqId, IBApi.SoftDollarTier[] tiers) {
        throw new NotImplementedException();
    }

    public void softDollarTiersProtoBuf (SoftDollarTiers softDollarTiersProto) {
        throw new NotImplementedException();
    }

    public void symbolSamples (int reqId, IBApi.ContractDescription[] contractDescriptions) {
        throw new NotImplementedException();
    }

    public void symbolSamplesProtoBuf (SymbolSamples symbolSamplesProto) {
        throw new NotImplementedException();
    }

    public void tickByTickAllLast (int reqId, int tickType, long time, double price, decimal size, IBApi.TickAttribLast tickAttribLast, string exchange, string specialConditions) {
        throw new NotImplementedException();
    }

    public void tickByTickBidAsk (int reqId, long time, double bidPrice, double askPrice, decimal bidSize, decimal askSize, IBApi.TickAttribBidAsk tickAttribBidAsk) {
        throw new NotImplementedException();
    }

    public void tickByTickDataProtoBuf (TickByTickData tickByTickDataProto) {
        throw new NotImplementedException();
    }

    public void tickByTickMidPoint (int reqId, long time, double midPoint) {
        throw new NotImplementedException();
    }

    public void tickEFP (int tickerId, int tickType, double basisPoints, string formattedBasisPoints, double impliedFuture, int holdDays, string futureLastTradeDate, double dividendImpact, double dividendsToLastTradeDate) {
        throw new NotImplementedException();
    }

    public void tickGeneric (int tickerId, int field, double value) {
        throw new NotImplementedException();
    }

    public void tickGenericProtoBuf (TickGeneric tickGenericProto) {
        throw new NotImplementedException();
    }

    public void tickNews (int tickerId, long timeStamp, string providerCode, string articleId, string headline, string extraData) {
        throw new NotImplementedException();
    }

    public void tickNewsProtoBuf (TickNews tickNewsProto) {
        throw new NotImplementedException();
    }

    public void tickOptionComputation (int tickerId, int field, int tickAttrib, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice) {
        throw new NotImplementedException();
    }

    public void tickOptionComputationProtoBuf (TickOptionComputation tickOptionComputationProto) {
        throw new NotImplementedException();
    }

    public void tickPrice (int tickerId, int field, double price, TickAttrib attribs) {
        throw new NotImplementedException();
    }

    public void tickPriceProtoBuf (TickPrice tickPriceProto) {
        throw new NotImplementedException();
    }

    public void tickReqParams (int tickerId, double minTick, string bboExchange, int snapshotPermissions) {
        throw new NotImplementedException();
    }

    public void tickReqParamsProtoBuf (TickReqParams tickReqParamsProto) {
        throw new NotImplementedException();
    }

    public void tickSize (int tickerId, int field, decimal size) {
        throw new NotImplementedException();
    }

    public void tickSizeProtoBuf (TickSize tickSizeProto) {
        throw new NotImplementedException();
    }

    public void tickSnapshotEnd (int tickerId) {
        throw new NotImplementedException();
    }

    public void tickSnapshotEndProtoBuf (TickSnapshotEnd tickSnapshotEndProto) {
        throw new NotImplementedException();
    }

    public void tickString (int tickerId, int field, string value) {
        throw new NotImplementedException();
    }

    public void tickStringProtoBuf (TickString tickStringProto) {
        throw new NotImplementedException();
    }

    public void updateAccountTime (string timestamp) {
        throw new NotImplementedException();
    }

    public void updateAccountTimeProtoBuf (AccountUpdateTime accountUpdateTimeProto) {
        throw new NotImplementedException();
    }

    public void updateAccountValue (string key, string value, string currency, string accountName) {
        throw new NotImplementedException();
    }

    public void updateAccountValueProtoBuf (AccountValue accountValueProto) {
        throw new NotImplementedException();
    }

    public void updateConfigResponseProtoBuf (UpdateConfigResponse updateConfigResponseProto) {
        throw new NotImplementedException();
    }

    public void updateMarketDepthL2ProtoBuf (MarketDepthL2 marketDepthL2Proto) {
        throw new NotImplementedException();
    }

    public void updateMarketDepthProtoBuf (MarketDepth marketDepthProto) {
        throw new NotImplementedException();
    }

    public void updateMktDepth (int tickerId, int position, int operation, int side, double price, decimal size) {
        throw new NotImplementedException();
    }

    public void updateMktDepthL2 (int tickerId, int position, string marketMaker, int operation, int side, double price, decimal size, bool isSmartDepth) {
        throw new NotImplementedException();
    }

    public void updateNewsBulletin (int msgId, int msgType, string message, string origExchange) {
        throw new NotImplementedException();
    }

    public void updateNewsBulletinProtoBuf (NewsBulletin newsBulletinProto) {
        throw new NotImplementedException();
    }

    public void updatePortfolio (IBApi.Contract contract, decimal position, double marketPrice, double marketValue, double averageCost, double unrealizedPNL, double realizedPNL, string accountName) {
        throw new NotImplementedException();
    }

    public void updatePortfolioProtoBuf (PortfolioValue portfolioValueProto) {
        throw new NotImplementedException();
    }

    public void userInfo (int reqId, string whiteBrandingId) {
        throw new NotImplementedException();
    }

    public void userInfoProtoBuf (UserInfo userInfoProto) {
        throw new NotImplementedException();
    }

    public void verifyAndAuthCompleted (bool isSuccessful, string errorText) {
        throw new NotImplementedException();
    }

    public void verifyAndAuthMessageAPI (string apiData, string xyzChallenge) {
        throw new NotImplementedException();
    }

    public void verifyCompleted (bool isSuccessful, string errorText) {
        throw new NotImplementedException();
    }

    public void verifyCompletedProtoBuf (VerifyCompleted verifyCompletedProto) {
        throw new NotImplementedException();
    }

    public void verifyMessageAPI (string apiData) {
        throw new NotImplementedException();
    }

    public void verifyMessageApiProtoBuf (VerifyMessageApi verifyMessageApiProto) {
        throw new NotImplementedException();
    }

    public void wshEventData (int reqId, string dataJson) {
        throw new NotImplementedException();
    }

    public void wshEventDataProtoBuf (IBApi.protobuf.WshEventData wshEventDataProto) {
        throw new NotImplementedException();
    }

    public void wshMetaData (int reqId, string dataJson) {
        throw new NotImplementedException();
    }

    public void wshMetaDataProtoBuf (WshMetaData wshMetaDataProto) {
        throw new NotImplementedException();
    }
}
