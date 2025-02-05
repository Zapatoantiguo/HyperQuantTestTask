using HyperQuantTestTask.BitfinexLib;
using HyperQuantTestTask.BitfinexLib.Enums;
using HyperQuantTestTask.BitfinexLib.Model;
using HyperQuantTestTask.BitfinexLib.Validation;
using Microsoft.AspNetCore.Mvc;

namespace HyperQuantTestTask.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitfinexConnectorRestController : ControllerBase
    {
        private ITestConnector _connector;
        public BitfinexConnectorRestController(ITestConnector connector)
        {
            _connector = connector;
        }

        [HttpGet]
        [Route("GetTrades")]
        [ProducesResponseType((int)StatusCodes.Status200OK, Type = typeof(IEnumerable<Trade>))]
        [ProducesResponseType((int)StatusCodes.Status400BadRequest)]
        public async Task<IResult> GetTrades(string symbol = "tBTCUSD", int count = 125)
        {
            if (!SymbolValidator.IsValidTradingPair(symbol))
                return TypedResults.BadRequest("Некорректный символ. ");

            var trades = await _connector.GetNewTradesAsync(symbol, count);

            return TypedResults.Ok(trades);
        }

        [HttpGet]
        [Route("GetCandles")]
        [ProducesResponseType((int)StatusCodes.Status200OK, Type = typeof(IEnumerable<Candle>))]
        [ProducesResponseType((int)StatusCodes.Status400BadRequest)]
        public async Task<IResult> GetCandles(string symbol = "tBTCUSD", BitfinexTimeFrame timeFrame=BitfinexTimeFrame._1m,
            DateTimeOffset? from = null, DateTimeOffset? to = null, int count = 100)
        {
            if (!SymbolValidator.IsValidTradingPair(symbol))
                return TypedResults.BadRequest("Некорректный символ. ");

            var candles = await _connector.GetCandleSeriesAsync(symbol, timeFrame, from, to, count);

            return TypedResults.Ok(candles);
        }

        [HttpGet]
        [Route("GetTicker")]
        [ProducesResponseType((int)StatusCodes.Status200OK, Type = typeof(Ticker))]
        [ProducesResponseType((int)StatusCodes.Status400BadRequest)]
        public async Task<IResult> GetTicker(string symbol = "tBTCUSD")
        {
            if (!SymbolValidator.IsValidTradingPair(symbol))
                return TypedResults.BadRequest("Некорректный символ. ");

            var ticker = await _connector.GetTickerAsync(symbol);

            return TypedResults.Ok(ticker);
        }
    }
}
