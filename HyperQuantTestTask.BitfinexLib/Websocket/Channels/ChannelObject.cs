using HyperQuantTestTask.BitfinexLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Websocket.Channels
{
    /// <summary>
    /// Содержит данные канала Bitfinex WebSocket API
    /// </summary>
    public abstract class ChannelObject
    {
        public long Id { get; set; }

        public DateTimeOffset LastHeartbeatTimestamp { get; set; }

        public abstract string GetTradingPair();

        /// <summary>
        /// Формирует ключ-параметр, идентифицирующий подписку в рамках данной категории.
        /// Например, tBTCUSD (т.е. symbol) для trades, trade:1m:tBTCUSD (т.н. key) для Candles
        /// </summary>
        public abstract string GetLiteralKey();
    }
}
