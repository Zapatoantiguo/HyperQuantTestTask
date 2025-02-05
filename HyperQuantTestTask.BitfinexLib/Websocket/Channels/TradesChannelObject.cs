using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Websocket.Channels
{
    public class TradesChannelObject : ChannelObject
    {
        public string TradingPair { get; private set; }

        /// <summary>
        /// Ключ для обозначения канала данных по трейдам у Bitfinex
        /// </summary>
        public static readonly string ChannelLiteral = "trades";

        public TradesChannelObject(long id, string symbol)
        {
            TradingPair = symbol;
            Id = id;
        }

        public override string GetLiteralKey()
        {
            return TradingPair;
        }
        public override string GetTradingPair()
        {
            return TradingPair;
        }
    }
}
