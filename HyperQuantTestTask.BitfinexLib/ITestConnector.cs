using HyperQuantTestTask.BitfinexLib.Enums;
using HyperQuantTestTask.BitfinexLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib
{
    public interface ITestConnector
    {
        #region ImportantComment
        /*
         В предложенной модели Candle мне видится недочет: наряду со свойством Pair там должно присутствовать свойство
         TimeFrame (за какой интервал эта свеча). Как я понял бизнес-область интервал - это неотъемлемое качество свечи.
         Особенно явно этот недочет проявляется при подписке на события по свечам:
         можно подписаться на множество уведомлений по свечам одной торговой пары, но разных интервалов.
         Трогать предложенную модель я не стал, но, кажется, это должно быть сделано.
         */
        #endregion

        #region Rest
        Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount);

        /// <remarks>
        /// Исправлен тип параметра для указания time frame, поскольку API поддерживает только конечное множество интервалов.
        /// Параметр long? count = 0 был не очень ясен: если есть значение по умолчанию, то зачем nullable и почему по умолчанию
        /// 0? Я заменил тип и значение по умолчанию на используемое в самом API. Кроме того, параметр DateTimeOffset? from
        /// выглядит как кандидат на присвоение значения по умолчанию = null, т.к. в REST API он не является обязательным
        /// </remarks>
        Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, BitfinexTimeFrame timeFrame, 
            DateTimeOffset? from, DateTimeOffset? to = null, int count = 100);

        /// <remarks>
        /// В задании указан функционал получения тикера, поэтому я добавил это в интерфейс
        /// </remarks>
        Task<Ticker> GetTickerAsync(string pair);

        #endregion

        #region Socket

        /// <remarks>
        /// Websocket API при уведомлении о сделке указывает тип операции: executed / updated. Из имен события неясно,
        /// нужны ли оба или только один. В текущей реализации операции обоих типов вызывают событие
        /// </remarks>
        event Action<Trade> NewBuyTrade;

        /// <remarks>
        /// Websocket API при уведомлении о сделке указывает тип операции: executed / updated. Из имен события неясно,
        /// нужны ли оба или только один. В текущей реализации операции обоих типов вызывают событие
        /// </remarks>
        event Action<Trade> NewSellTrade;

        /// <remarks>
        /// Удален параметр int maxCount = 100: в контексте данных по трейдам из WebSocket приходит либо Snapshot
        /// (формируемый после подписки) без возможности указания его размерности, либо обновление для одного трейда.
        /// Кажется маловероятным, что параметр предусматривался для ограничения записей из Snapshot, поэтому посчитал
        /// правильным его удалить.
        /// Кроме того, в общем случае лучше бы было возвращать Task (подписка подразумевает отправку ws-запроса, что-то может
        /// пойти не так, лучше бы, чтобы от этом можно было узнать). Но нормальной обработки ошибок у меня нет
        /// </remarks>
        void SubscribeTrades(string pair);
        void UnsubscribeTrades(string pair);

        event Action<Candle> CandleSeriesProcessing;

        /// <remarks>
        /// Удален параметр long? count = 0: в контексте данных по свечам из WebSocket приходит либо Snapshot
        /// (формируемый после подписки) без возможности указания его размерности, либо обновление для одной свечи.
        /// Кажется маловероятным, что параметр предусматривался для ограничения записей из Snapshot, поэтому посчитал
        /// правильным его удалить. 
        /// Также удалены параметры для задания временного промежутка: в подписке он не имеет смысла (разве что какой-то 
        /// неочевидный смысл для фильтрации Snapshot...)
        /// Также исправлен тип параметра для указания time frame, поскольку API поддерживает только конечное множество интервалов.
        /// Кроме того, в общем случае лучше бы было возвращать Task (подписка подразумевает отправку ws-запроса, что-то может
        /// пойти не так, лучше бы, чтобы от этом можно было узнать). Но нормальной обработки ошибок у меня нет
        /// </remarks>
        void SubscribeCandles(string pair, BitfinexTimeFrame timeFrame);
        void UnsubscribeCandles(string pair);

        #endregion
    }
}
