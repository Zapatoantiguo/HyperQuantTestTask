using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Websocket
{
    public class CandlesSubscribeRequest
    {
        public string Event { get; init; } = "subscribe";
        public string Channel { get; init; } = "candles";
        public string Key { get; set; } = "trade:1m:tBTCUSD";

    }
}
