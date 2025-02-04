using HyperQuantTestTask.BitfinexLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Websocket.Events
{
    public class CandleNotificationEventArgs : EventArgs
    {
        public Candle Candle { get; set; } = null!;
    }
}
