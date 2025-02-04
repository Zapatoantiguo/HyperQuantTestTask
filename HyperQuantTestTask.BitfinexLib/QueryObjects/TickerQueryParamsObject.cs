using HyperQuantTestTask.BitfinexLib.Exceptions;
using HyperQuantTestTask.BitfinexLib.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.QueryObjects
{
    public class TickerQueryParamsObject : IQueryParamsObject
    {
        public string TradingPair {get; private set;}
        
        public TickerQueryParamsObject(string tradingPair)
        {
            TradingPair = tradingPair;
        }
        public string ToUrlPart()
        {
            if (!IsValid())
                throw new QueryParamsValidationException("Некорректные параметры запроса тикера");

            return $"ticker/{TradingPair}";
        }

        public bool IsValid()
        {
            return SymbolValidator.IsValidTradingPair(TradingPair);
        }
    }
}
