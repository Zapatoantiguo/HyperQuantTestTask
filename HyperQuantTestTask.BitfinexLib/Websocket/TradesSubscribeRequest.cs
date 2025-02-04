namespace HyperQuantTestTask.BitfinexLib.Websocket
{
    public class TradesSubscribeRequest
    {
        public string Event { get; init; } = "subscribe";
        public string Channel { get; init; } = "trades";
        public string Symbol { get; set; } = "tBTCUSD";
    }

    
}
