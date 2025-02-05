using HyperQuantTestTask.BitfinexLib;
using HyperQuantTestTask.BitfinexLib.Model;
using Microsoft.AspNetCore.Mvc;

namespace HyperQuantTestTask.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BalanceCalcController : ControllerBase
    {
        ITestConnector _connector;

        public BalanceCalcController(ITestConnector connector)
        {
            _connector = connector;
        }

        [HttpGet]
        [ProducesResponseType((int)StatusCodes.Status200OK, Type = typeof(Balance))]
        public async Task<IResult> Get(decimal btc = 1, decimal xrp = 15000, decimal xmr = 50, decimal dash = 30)
        {
            var task1 = _connector.GetTickerAsync("tBTCUSD");
            var task2 = _connector.GetTickerAsync("tXRPUSD");
            var task3 = _connector.GetTickerAsync("tXMRUSD");
            var task4 = _connector.GetTickerAsync("tDSHUSD");

            await Task.WhenAll(task1, task2, task3, task4);

            var balance = new Balance { BtcAmount = btc, XrpAmount = xrp, XmrAmount = xmr, DashAmount = dash };

            var btcrate = task1.Result.LastPrice;
            var xrprate = task2.Result.LastPrice;
            var xmrrate = task3.Result.LastPrice;
            var dshrate = task4.Result.LastPrice;

            balance.TotalUsd = btcrate * btc + xrprate * xrp + xmrrate * xmr + dshrate * dash;

            balance.TotalBtc = btc + xrprate * xrp / btcrate + xmrrate * xmr / btcrate + dshrate * dash / btcrate;

            balance.TotalXrp = btc * btcrate / xrprate + xrp + xmrrate * xmr / xrprate + dshrate * dash / xrprate;

            balance.TotalXmr = btc * btcrate / xmrrate + xrp * xrprate / xmrrate + xmr + dshrate * dash / xmrrate;

            balance.TotalDash = btc * btcrate / dshrate + xrp * xrprate / dshrate + xmr * xmrrate / dshrate + dash;

            return TypedResults.Ok(balance);
        }
    }
}
