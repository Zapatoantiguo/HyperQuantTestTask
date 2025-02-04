using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Websocket
{
    public class UnsubscribeRequest
    {
        public string Event { get; init; } = "unsubscribe";
        public long ChanId { get; init; }
    }
}
