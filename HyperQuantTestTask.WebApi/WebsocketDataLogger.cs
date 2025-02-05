using HyperQuantTestTask.BitfinexLib;
using HyperQuantTestTask.BitfinexLib.Model;

namespace HyperQuantTestTask.WebApi
{
    public class WebsocketDataLogger
    {
        ILogger<DummyLog> _logger;
        ITestConnector _connector;

        public WebsocketDataLogger(ILogger<DummyLog> logger, ITestConnector connector)
        {
            _logger = logger;
            _connector = connector;
        }

        public void SubscribeConsoleToConnectorEvents()
        {
            _connector.NewBuyTrade += trade =>
            {
                _logger.LogInformation($"New buy trade. Id: {trade.Id}, Symbol: {trade.Pair}, Timestamp: {trade.Time}," +
                    $" Amount: {trade.Amount}");
            };

            _connector.NewSellTrade += trade =>
            {
                _logger.LogInformation($"New sell trade. Id: {trade.Id}, Symbol: {trade.Pair}, Timestamp: {trade.Time}," +
                    $" Amount: {trade.Amount}");
            };

            _connector.CandleSeriesProcessing += candle =>
            {
                _logger.LogInformation($"New candle data. Symbol: {candle.Pair}, OpnTime: {candle.OpenTime}," +
                $" LPrice: {candle.LowPrice}, HPrice: {candle.HighPrice}, OpenPrice: {candle.OpenPrice}, " +
                $" Volume: {candle.TotalVolume}");
            };
        }



        public class DummyLog
        {

        }

        
    }
}
