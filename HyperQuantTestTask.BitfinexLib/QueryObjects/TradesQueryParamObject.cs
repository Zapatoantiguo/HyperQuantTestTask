using HyperQuantTestTask.BitfinexLib.Enums;
using HyperQuantTestTask.BitfinexLib.Exceptions;
using HyperQuantTestTask.BitfinexLib.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.QueryObjects
{
    public class TradesQueryParamObject : IQueryParamsObject
    {
        static readonly int maxResponseRecords = 10000;
        public string TradingPair { get; private set; }
        public int Limit {  get; private set; }
        public SortQueryParam Sort { get; private set; }
        public long StartTimestamp { get; private set; }
        public long EndTimestamp {  get; private set; }
        public TradesQueryParamObject(string traidingPair, int limit = 125, SortQueryParam sort = SortQueryParam.Descending,
            long? startTimestamp = 0, long? endTimestamp = 0)
        {
            TradingPair = traidingPair;
            Limit = limit;
            Sort = sort;
            StartTimestamp = startTimestamp ?? 0;
            EndTimestamp = endTimestamp ?? 0;
        }
        public string ToUrlPart()
        {
            if (!IsValid())
                throw new QueryParamsValidationException("Некорректные параметры запроса трейдов");

            string startTsStr = StartTimestamp == 0 ? string.Empty : StartTimestamp.ToString();
            string endTsStr = EndTimestamp == 0 ? string.Empty : EndTimestamp.ToString();

            return $"trades/{TradingPair}/hist?limit={Limit}&sort={(int)Sort}&start={startTsStr}&end={endTsStr}";
        }

        public bool IsValid()
        {
            bool timestampsAreCorrect = StartTimestamp >= 0 && EndTimestamp >= 0 &&
                (StartTimestamp == 0 || EndTimestamp == 0 ||
                EndTimestamp > StartTimestamp);

            return SymbolValidator.IsValidTradingPair(TradingPair) &&
                timestampsAreCorrect &&
                Limit > 0 && Limit < maxResponseRecords;
        }
    }
}
