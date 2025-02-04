using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Enums
{
    /// <summary>
    /// Доступные значения параметра типа Time Frame в API Bitfinex.
    /// </summary>

    public enum BitfinexTimeFrame
    {
        _1m,
        _5m,
        _15m,
        _30m,
        _1h,
        _3h,
        _6h,
        _12h,
        _1D,
        _1W,
        _14D,
        _1M  
    }
    public static class StupidExtensions
    {   
        public static string ToLiteral(this BitfinexTimeFrame timeFrame) => timeFrameLiterals[timeFrame];

        public static int ToSeconds(this BitfinexTimeFrame timeFrame) => timeFrameSecIntervals[timeFrame];

        public static BitfinexTimeFrame FromLiteral(this BitfinexTimeFrame timeFrame, string literal)
        {
            return timeFrameLiterals.Single(kvp => kvp.Value == literal).Key;
        }

        private readonly static Dictionary<BitfinexTimeFrame, string> timeFrameLiterals = new()
        {
            {BitfinexTimeFrame._1m, "1m"},
            {BitfinexTimeFrame._5m, "5m"},
            {BitfinexTimeFrame._15m, "15m"},
            {BitfinexTimeFrame._30m, "30m"},
            {BitfinexTimeFrame._1h, "1h"},
            {BitfinexTimeFrame._3h, "3h"},
            {BitfinexTimeFrame._6h, "6h"},
            {BitfinexTimeFrame._12h, "12h"},
            {BitfinexTimeFrame._1D, "1D"},
            {BitfinexTimeFrame._1W, "1W"},
            {BitfinexTimeFrame._14D, "14D"},
            {BitfinexTimeFrame._1M, "1M"} // TODO: dynamic month
        };

        private readonly static Dictionary<BitfinexTimeFrame, int> timeFrameSecIntervals = new()
        {
            {BitfinexTimeFrame._1m, 60*1},
            {BitfinexTimeFrame._5m, 60*5},
            {BitfinexTimeFrame._15m, 60*15},
            {BitfinexTimeFrame._30m, 60*30},
            {BitfinexTimeFrame._1h, 60*60},
            {BitfinexTimeFrame._3h, 60*60*3},
            {BitfinexTimeFrame._6h, 60*60*6},
            {BitfinexTimeFrame._12h, 60*60*12},
            {BitfinexTimeFrame._1D, 60*60*24},
            {BitfinexTimeFrame._1W, 60*60*24*7},
            {BitfinexTimeFrame._14D, 60*60*24*14},
            {BitfinexTimeFrame._1M, 60*60*24*30} // TODO: dynamic month
        };
    }

}
