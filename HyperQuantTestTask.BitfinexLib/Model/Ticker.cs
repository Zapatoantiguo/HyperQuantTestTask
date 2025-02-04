using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitfinexTests.Model
{
    public class Ticker
    {
        /// <summary>
        /// Валютная пара
        /// </summary>
        public string Pair { get; set; }

        /// <summary>
        /// Price of last highest bid
        /// </summary>
        public decimal Bid { get; set; }

        /// <summary>
        /// Sum of the 25 highest bid sizes
        /// </summary>
        public decimal BidSize { get; set; }

        /// <summary>
        /// Price of last lowest ask
        /// </summary>
        public decimal Ask { get; set; }

        /// <summary>
        /// Sum of the 25 lowest ask sizes
        /// </summary>
        public decimal AskSize { get; set; }

        /// <summary>
        /// Изменение последней цены по сравнению со вчерашней
        /// </summary>
        public decimal DailyChange { get; set; }

        /// <summary>
        /// Относительное изменение цены со вчерашнего дня
        /// </summary>
        public decimal DailyChangeRelative { get; set; }

        /// <summary>
        /// Дневной объем
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Цена последней сделки
        /// </summary>
        public decimal LastPrice { get; set; }

        /// <summary>
        /// Максимальная цена за день
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// Минимальная цена за день
        /// </summary>
        public decimal Low { get; set; }

    }
}
