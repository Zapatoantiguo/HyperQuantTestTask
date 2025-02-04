using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Enums
{
    /// <summary>
    /// Тип события сделки (trade executed / trade execution update) 
    /// </summary>
    public enum TradeEventType
    {
        Executed,
        Update
    }
}
