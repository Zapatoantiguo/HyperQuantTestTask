using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Model
{
    public class Candle
    {
        /// <summary>
        /// Валютная пара
        /// </summary>
        public string Pair { get; set; }

        /// <summary>
        /// Цена открытия
        /// </summary>
        public decimal OpenPrice { get; set; }

        /// <summary>
        /// Максимальная цена
        /// </summary>
        public decimal HighPrice { get; set; }

        /// <summary>
        /// Минимальная цена
        /// </summary>
        public decimal LowPrice { get; set; }

        /// <summary>
        /// Цена закрытия
        /// </summary>
        public decimal ClosePrice { get; set; }

        /// <summary>
        /// Partial (Общая сумма сделок)
        /// </summary>
        /// <remarks>
        /// Неясно, что это за поле, возможно, опечатка в задаче. Пока возвращает -1
        /// </remarks>
        public decimal TotalPrice { get => -1; }

        /// <summary>
        /// Partial (Общий объем)
        /// </summary>
        public decimal TotalVolume { get; set; }

        /// <summary>
        /// Время
        /// </summary>
        public DateTimeOffset OpenTime { get; set; }
    }
}
