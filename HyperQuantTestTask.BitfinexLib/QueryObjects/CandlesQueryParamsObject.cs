using HyperQuantTestTask.BitfinexLib.Enums;
using HyperQuantTestTask.BitfinexLib.Exceptions;
using HyperQuantTestTask.BitfinexLib.Extensions;
using HyperQuantTestTask.BitfinexLib.Model;
using HyperQuantTestTask.BitfinexLib.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.QueryObjects
{
    // TODO: добавить поддержку funding-операций
    // TODO: добавить поддержку использования параметра "last"
    public class CandlesQueryParamsObject : IQueryParamsObject
    {
        static readonly int maxResponseRecords = 10000;

        public string TradingPair { get; private set; }
        public int Limit { get; private set; }
        public SortQueryParam Sort { get; private set; }
        public long StartTimestamp { get; private set; }
        public long EndTimestamp { get; private set; }
        public BitfinexTimeFrame TimeFrame { get; private set; }

        public CandlesQueryParamsObject(string traidingPair, int limit = 100, SortQueryParam sort = SortQueryParam.Descending,
            long? startTimestamp = 0, long? endTimestamp = 0, BitfinexTimeFrame timeFrame = BitfinexTimeFrame._1m)
        {
            TradingPair = traidingPair;
            Limit = limit;
            Sort = sort;
            StartTimestamp = startTimestamp ?? 0;
            EndTimestamp = endTimestamp ?? 0;
            TimeFrame = timeFrame;
        }

        public string ToUrlPart()
        {
            if (!IsValid())
                throw new QueryParamsValidationException("Некорректные параметры запроса свечей");

            string candleParam = new CandleSetting() { TradingPair = TradingPair, TimeFrame = TimeFrame }.ToString();

            string startTsStr = StartTimestamp == 0 ? string.Empty : StartTimestamp.ToString();
            string endTsStr = EndTimestamp == 0 ? string.Empty : EndTimestamp.ToString();

            return $"candles/{candleParam}/hist?sort={(int)Sort}&start={startTsStr}&end={endTsStr}&limit={Limit}";
        }

        public bool IsValid()
        {
            bool timestampsAreCorrect = StartTimestamp >= 0 && EndTimestamp >= 0 &&
                (StartTimestamp == 0 || EndTimestamp == 0 ||
                EndTimestamp > StartTimestamp);

            return SymbolValidator.IsValidTradingPair(TradingPair) &&
                timestampsAreCorrect &&
                Limit > 0 && Limit <= maxResponseRecords;
        }
    }
}
