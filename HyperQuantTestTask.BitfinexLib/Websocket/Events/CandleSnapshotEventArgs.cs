using HyperQuantTestTask.BitfinexLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Websocket.Events
{
    public class CandleSnapshotEventArgs : EventArgs
    {
        public List<Candle> Snapshot { get; set; }
    }
}
