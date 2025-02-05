using HyperQuantTestTask.BitfinexLib;
using HyperQuantTestTask.BitfinexLib.Enums;
using HyperQuantTestTask.BitfinexLib.Validation;
using Microsoft.AspNetCore.Mvc;

namespace HyperQuantTestTask.WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BitfinexConnectorWsController : ControllerBase
    {
        private ITestConnector _connector;

        public BitfinexConnectorWsController(ITestConnector connector)
        {
            _connector = connector;
        }

        [HttpPost]
        [Route("SubscribeTrades")]
        public async Task<IResult> SubscribeTrades(string symbol = "tBTCUSD")
        {
            if (!SymbolValidator.IsValidTradingPair(symbol))
                return TypedResults.BadRequest("Некорректный символ. ");

            _connector.SubscribeTrades(symbol);
            return TypedResults.Ok();
        }

        [HttpPost]
        [Route("UnsubscribeTrades")]
        public async Task<IResult> UnsubscribeTrades(string symbol = "tBTCUSD")
        {
            if (!SymbolValidator.IsValidTradingPair(symbol))
                return TypedResults.BadRequest("Некорректный символ. ");

            _connector.UnsubscribeTrades(symbol);
            return TypedResults.Ok();
        }

        [HttpPost]
        [Route("SubscribeCandles")]
        public async Task<IResult> SubscribeCandles(string symbol = "tBTCUSD", BitfinexTimeFrame timeFrame = BitfinexTimeFrame._1m)
        {
            if (!SymbolValidator.IsValidTradingPair(symbol))
                return TypedResults.BadRequest("Некорректный символ. ");

            _connector.SubscribeCandles(symbol, timeFrame);
            return TypedResults.Ok();
        }

        [HttpPost]
        [Route("UnsubscribeCandles")]
        public async Task<IResult> UnsubscribeCandles(string symbol = "tBTCUSD")
        {
            if (!SymbolValidator.IsValidTradingPair(symbol))
                return TypedResults.BadRequest("Некорректный символ. ");

            _connector.UnsubscribeCandles(symbol);
            return TypedResults.Ok();
        }


    }
}
