using HyperQuantTestTask.BitfinexLib.Enums;
using HyperQuantTestTask.BitfinexLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Websocket.Channels
{
    public class CandlesChannelObject : ChannelObject
    {
        /// <summary>
        /// Ключ для обозначения канала данных по свечам у Bitfinex
        /// </summary>
        public static readonly string ChannelLiteral = "candles";

        private CandleSetting _candleSetting;

        public string TradingPair { get => _candleSetting.TradingPair; }

        public CandlesChannelObject(long id, string key)
        {
            _candleSetting = CandleSetting.BuildFromKey(key);
            Id = id;
        }

        public override string GetLiteralKey()
        {
            return _candleSetting.ToString();
        }

        public override string GetTradingPair()
        {
            return TradingPair;
        }
    }
}
