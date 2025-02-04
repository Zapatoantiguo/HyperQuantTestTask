using HyperQuantTestTask.BitfinexLib.Enums;


namespace HyperQuantTestTask.BitfinexLib.Model
{
    /// <summary>
    /// Представление для параметра запросов по свечам.
    /// В документации такой параметр называется key (например, trade:1m:tBTCUSD)
    /// </summary>
    /// TODO: добавить поддержку ключей для Funding currency candles и Aggregate funding currency candles
    public class CandleSetting
    {
        public string CategoryLiteral { get; } = "trade";
        public BitfinexTimeFrame TimeFrame { get; set; }
        public string TradingPair { get; set; }

        public static CandleSetting BuildFromKey(string key)
        {
            // TODO: добавить валидацию

            var parts = key.Split(':');
            BitfinexTimeFrame tf = BitfinexTimeFrame._1m;
            tf.FromLiteral(parts[1]);
            string pair = parts[2];

            return new CandleSetting
            {
                TradingPair = pair,
                TimeFrame = tf
            };
        }

        /// <summary>
        /// Формирует строку настроек свечи типа trade:1m:tBTCUSD
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{CategoryLiteral}:{TimeFrame.ToLiteral()}:{TradingPair}";
        }
    }
}
