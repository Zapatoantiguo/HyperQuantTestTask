using HyperQuantTestTask.BitfinexLib.Enums;
using HyperQuantTestTask.BitfinexLib.Model;


namespace HyperQuantTestTask.BitfinexLib.Websocket.Events
{
    /// <summary>
    /// Параметры события уведомления о новой сделке. В зависимости от типа события обозначает trade executed или trade updated
    /// </summary>
    public class TradeNotificationEventArgs : EventArgs
    {
        public TradeEventType Type { get; set; }
        public Trade Trade { get; set; } = null!;
    }
}
