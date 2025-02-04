using HyperQuantTestTask.BitfinexLib.Dto;
using HyperQuantTestTask.BitfinexLib.Model;


namespace HyperQuantTestTask.BitfinexLib.Extensions
{
    public static class DtoExtensions
    {
        public static Ticker ToEntity(this TickerDto dto, string pair)
        {
            return new Ticker
            {
                Pair = pair,
                Bid = dto.Bid,
                BidSize = dto.BidSize,
                Ask = dto.Ask,
                AskSize = dto.AskSize,
                DailyChange = dto.DailyChange,
                DailyChangeRelative = dto.DailyChangeRelative,
                High = dto.High,
                Low = dto.Low,
                LastPrice = dto.LastPrice,
                Volume = dto.Volume
            };
        }

        public static Trade ToEntity(this TradeDto dto, string pair)
        {
            return new Trade
            {
                Pair = pair,
                Price = dto.Price,
                Amount = dto.Amount,
                Time = DateTimeOffset.FromUnixTimeMilliseconds(dto.Timestamp),
                Id = dto.Id.ToString()
            };
        }

        public static Candle ToEntity(this CandleDto dto, string pair)
        {
            return new Candle
            {
                Pair = pair,
                OpenPrice = dto.OpenPrice,
                ClosePrice = dto.ClosePrice,
                HighPrice = dto.HighPrice,
                LowPrice = dto.LowPrice,
                TotalVolume = dto.Volume,
                OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(dto.Timestamp),
                
            };
        }

    }
}
