using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Dto
{
    public class TradeDto
    {
        /// <summary>
        /// Trade ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// UNIX Millisecond time stamp
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Amount bought (positive) or sold (negative).
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Price at which the trade was executed
        /// </summary>
        public decimal Price { get; set; }
    }
}
