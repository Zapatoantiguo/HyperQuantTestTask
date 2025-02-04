using HyperQuantTestTask.BitfinexLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Websocket.Events
{
    public class TradeSnapshotEventArgs : EventArgs
    {
        public List<Trade> Snapshot { get; set; } = new();
    }
}
