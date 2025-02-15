﻿using HyperQuantTestTask.BitfinexLib.Enums;
using HyperQuantTestTask.BitfinexLib.Model;

namespace HyperQuantTestTask.BitfinexLib 
{
    public class TestConnector : ITestConnector
    {
        private BitfinexRestApiClient _restClient;
        private BitfinexWsClient _wsClient;

        public TestConnector(BitfinexRestApiClient restClient, BitfinexWsClient wsClient)
        {
            _restClient = restClient;
            _wsClient = wsClient;
            ConnectEvents();

            // Управление коннектами и реконнектами для веб-сокета отдельная тема, которую нужно тщательно продумать
            // Здесь, в тестовом задании, используется топорный блокирующий вызов подключения в конструкторе
            _wsClient.ConnectAsync().Wait();
        }

        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;
        public event Action<Candle> CandleSeriesProcessing;

        public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
        {
            var result = await _restClient.GetTradesAsync(pair, maxCount);
            return result;
        }

        public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, BitfinexTimeFrame timeFrame, 
            DateTimeOffset? from, DateTimeOffset? to = null, int count = 100)
        {
            var result = await _restClient.GetCandlesAsync(pair, count, timeFrame: timeFrame, start: from, end: to);
            return result;
        }

        public async Task<Ticker> GetTickerAsync(string pair)
        {
            var result = await _restClient.GetTickerAsync(pair);
            return result;
        }

        public async void SubscribeCandles(string pair, BitfinexTimeFrame timeFrame)
        {
            await _wsClient.SubscribeCandlesAsync(new() { TradingPair = pair, TimeFrame = timeFrame});
        }

        public async void SubscribeTrades(string pair)
        {
            await _wsClient.SubscribeTradesAsync(pair);
        }

        public async void UnsubscribeCandles(string pair)
        {
            await _wsClient.UnsubscribeCandlesAsync(pair);
        }

        public async void UnsubscribeTrades(string pair)
        {
            await _wsClient.UnsubscribeTradesAsync(pair);
        }

        public async void RebootSocket()
        {
            await _wsClient.DisconnectAsync();
            await _wsClient.ConnectAsync();
        }

        private void ConnectEvents()
        {
            _wsClient.OnTradeReceived += (_, args) =>
            {
                var trade = args.Trade;
                if (args.Type == TradeEventType.Update) return;
                if (trade.Side == "buy") NewBuyTrade?.Invoke(trade);
                else if (trade.Side == "sell") NewSellTrade?.Invoke(trade);
            };

            _wsClient.OnCandleReceived += (_, args) =>
            {
                var candle = args.Candle;
                CandleSeriesProcessing?.Invoke(candle);
            };
        }
        
    }
}
