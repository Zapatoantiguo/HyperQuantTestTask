using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Dto
{
    public class CandleDto
    {
        /// <summary>
        /// UNIX millisecond time stamp
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// First execution during the time frame
        /// </summary>
        public long OpenPrice { get; set; }

        /// <summary>
        /// Last execution during the time frame
        /// </summary>
        public long ClosePrice { get; set; }

        /// <summary>
        /// Highest execution during the time frame
        /// </summary>
        public long HighPrice { get; set; }

        /// <summary>
        /// Lowest execution during the timeframe
        /// </summary>
        public long LowPrice { get; set; }

        /// <summary>
        /// Quantity of symbol traded within the timeframe
        /// </summary>
        public decimal Volume { get; set; }
    }
}
