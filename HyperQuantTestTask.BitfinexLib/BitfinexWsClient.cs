using HyperQuantTestTask.BitfinexLib.Converters;
using HyperQuantTestTask.BitfinexLib.Dto;
using HyperQuantTestTask.BitfinexLib.Enums;
using HyperQuantTestTask.BitfinexLib.Exceptions;
using HyperQuantTestTask.BitfinexLib.Extensions;
using HyperQuantTestTask.BitfinexLib.Model;
using HyperQuantTestTask.BitfinexLib.Validation;
using HyperQuantTestTask.BitfinexLib.Websocket;
using HyperQuantTestTask.BitfinexLib.Websocket.Channels;
using HyperQuantTestTask.BitfinexLib.Websocket.Events;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace HyperQuantTestTask.BitfinexLib
{
    public class BitfinexWsClient : IDisposable
    {
        private ClientWebSocket _client;
        private CancellationTokenSource _cts;
        private SemaphoreSlim _sendSemaphore;

        private Dictionary<long, ChannelObject> _channels = new();


        public int ReceiveBufferSize { get; set; } = 8192;
        public WebSocketState? SocketState { get => _client?.State; }

        public event EventHandler<TradeSnapshotEventArgs> OnTradeSnapshotReceived;
        public event EventHandler<TradeNotificationEventArgs> OnTradeReceived;
        public event EventHandler<CandleSnapshotEventArgs> OnCandleSnapshotReceived;
        public event EventHandler<CandleNotificationEventArgs> OnCandleReceived;
        public List<ChannelObject> GetActiveChannels() => _channels.Select(kvp => kvp.Value).ToList();

        public async Task ConnectAsync(string url)
        {
            if (_client != null)
            {
                if (_client.State == WebSocketState.Open) return;
                else _client.Dispose();
            }

            _client = new ClientWebSocket();
            _channels.Clear();

            if (_cts != null) _cts.Dispose();
            _cts = new CancellationTokenSource();
            _sendSemaphore = new SemaphoreSlim(1, 1);

            await _client.ConnectAsync(new Uri(url), _cts.Token);
            await Task.Factory.StartNew(ReceiveLoop, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public async Task DisconnectAsync()
        {
            if (_client is null) return;

            if (_client.State == WebSocketState.Open)
            {
                _cts.CancelAfter(TimeSpan.FromSeconds(2));
                await _client.CloseOutputAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
                await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }

            _client.Dispose();
            _client = null;
            _cts.Dispose();
            _cts = null;
            _sendSemaphore.Dispose();
            _sendSemaphore = null;

            _channels.Clear();
        }

        private async Task ReceiveLoop()
        {
            var loopToken = _cts.Token;
            MemoryStream outputStream = null;
            WebSocketReceiveResult receiveResult = null;
            var buffer = new byte[ReceiveBufferSize];
            try
            {
                while (!loopToken.IsCancellationRequested)
                {
                    outputStream = new MemoryStream(ReceiveBufferSize);
                    do
                    {
                        receiveResult = await _client.ReceiveAsync(buffer, _cts.Token);
                        if (receiveResult.MessageType != WebSocketMessageType.Close)
                            outputStream.Write(buffer, 0, receiveResult.Count);
                    }
                    while (!receiveResult.EndOfMessage);

                    if (receiveResult.MessageType == WebSocketMessageType.Close) break;

                    outputStream.Position = 0;
                    ResponseReceived(outputStream);
                }
            }
            catch (TaskCanceledException) { }
            finally
            {
                outputStream?.Dispose();
            }
        }

        private async Task SendMessageAsync(string data)
        {
            var sendToken = _cts.Token;
            var bytes = Encoding.UTF8.GetBytes(data);

            // Разрешен только вызов из одного потока (см. раздел Remarks документации на метод ClientWebSocket.SendAsync)
            await _sendSemaphore.WaitAsync(sendToken);
            try
            {
                await _client.SendAsync(bytes, WebSocketMessageType.Binary, true, default);
            }
            catch (TaskCanceledException) { }
            finally
            {
                _sendSemaphore.Release();
            }
        }

        public async Task SubscribeTradesAsync(string pair)
        {
            if (!SymbolValidator.IsValidTradingPair(pair))
                throw new QueryParamsValidationException($"Попытка подписки на трейд с некорректным символом: {pair}");

            if (_client.State != WebSocketState.Open)
                throw new InvalidOperationException("Попытка взаимодействия с сервером при отсутствии Websocket-подключения");

            TradesSubscribeRequest request = new() { Symbol = pair };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            string json = JsonSerializer.Serialize(request, options);
            await SendMessageAsync(json);
        }

        public async Task UnsubscribeTradesAsync(string pair)
        {
            long? chanId = _channels.Values.SingleOrDefault(ch => ch is TradesChannelObject tco && tco.TradingPair == pair)?.Id;

            if (chanId == null)
                //throw new QueryParamsValidationException($"Отсутствует подписка на канал трейдов с символом: {pair}. Отписка невозможна.");
                return;

            if (_client.State != WebSocketState.Open)
                throw new InvalidOperationException("Попытка взаимодействия с сервером при отсутствии Websocket-подключения");

            UnsubscribeRequest request = new() { ChanId = chanId.Value };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            string json = JsonSerializer.Serialize(request, options);
            await SendMessageAsync(json);
        }

        public async Task SubscribeCandlesAsync(CandleSetting candleParam)
        {
            if (_client.State != WebSocketState.Open)
                throw new InvalidOperationException("Попытка взаимодействия с сервером при отсутствии Websocket-подключения");

            CandlesSubscribeRequest request = new() { Key = candleParam.ToString() };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            string json = JsonSerializer.Serialize(request, options);
            await SendMessageAsync(json);
        }

        public async Task UnsubscribeCandlesAsync(string pair)
        {
            long? chanId = _channels.Values.SingleOrDefault(ch => ch is CandlesChannelObject tco && tco.TradingPair == pair)?.Id;

            if (chanId == null)
                throw new QueryParamsValidationException($"Отсутствует подписка на канал свечей с символом: {pair}. Отписка невозможна.");

            if (_client.State != WebSocketState.Open)
                throw new InvalidOperationException("Попытка взаимодействия с сервером при отсутствии Websocket-подключения");

            UnsubscribeRequest request = new() { ChanId = chanId.Value };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            string json = JsonSerializer.Serialize(request, options);
            await SendMessageAsync(json);
        }

        private void ResponseReceived(Stream inputStream)
        {
            using (var reader = new StreamReader(inputStream, encoding: Encoding.UTF8, true))
            {
                ParseMessage(reader);
            }
        }

        // TODO: познакомиться с протоколом Bitfinex (форматы событий и данных и пр.) поближе и переписать это месиво ниже по уму
        private void ParseMessage(StreamReader reader)
        {
            char firstChar = (char)reader.Read();
            if (firstChar == '{') // event message
            {
                string json = firstChar.ToString() + reader.ReadToEnd();
                var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                string eventType = dictionary["event"].ToString();

                switch (eventType)
                {
                    case "subscribed":
                        {
                            string channel = dictionary["channel"].ToString();
                            long id = long.Parse(dictionary["chanId"].ToString());

                            ChannelObject channelObj;
                            if (channel == TradesChannelObject.ChannelLiteral)
                            {
                                string symbol = dictionary["symbol"].ToString();
                                channelObj = new TradesChannelObject(id, symbol) { LastHeartbeatTimestamp = DateTimeOffset.UtcNow };
                                _channels.Add(id, channelObj);
                            }

                            else if (channel == CandlesChannelObject.ChannelLiteral)
                            {
                                string key = dictionary["key"].ToString();
                                channelObj = new CandlesChannelObject(id, key) { LastHeartbeatTimestamp = DateTimeOffset.UtcNow };
                                _channels.Add(id, channelObj);
                            }
                            break;
                        }
                    case "unsubscribed":
                        {
                            long id = long.Parse(dictionary["chanId"].ToString());

                            _channels.Remove(id);
                            break;
                        }
                }
            }
            else if (firstChar == '[') // data message
            {
                string idStr = string.Empty;
                char next = (char)reader.Read();
                while (next != ',')
                {
                    if (!char.IsDigit(next)) throw new Exception("Ошибка в парсинге данных из WebSocket");
                    idStr += next;
                    next = (char)reader.Read();
                }
                long id = long.Parse(idStr);

                if (!_channels.ContainsKey(id))
                    return;

                string full = reader.ReadToEnd();

                if (full.Contains("hb"))    // channel heartbeat
                {
                    _channels[id].LastHeartbeatTimestamp = DateTimeOffset.UtcNow;
                    return;
                }
                TradeEventType newTradeType = TradeEventType.Update;
                if (full.Contains("te"))
                {
                    newTradeType = TradeEventType.Executed;
                    full = full.Remove(0, 5);
                }
                else if (full.Contains("tu"))
                {
                    newTradeType = TradeEventType.Update;
                    full = full.Remove(0, 5);
                }

                full = full.Remove(full.Length - 1);    // удаление закрывающего символа ], парного уже считанному [
                byte[] jsonBytes = Encoding.UTF8.GetBytes(full);
                Utf8JsonReader jsonReader = new Utf8JsonReader(jsonBytes);

                if (full.Contains("[["))
                    jsonReader.Read();

                if (_channels[id] is TradesChannelObject to)
                {
                    var converter = new TradesDtoJsonConverter();
                    var dtos = converter.Read(ref jsonReader, typeof(List<TradeDto>), new JsonSerializerOptions());
                    List<Trade> trades = dtos.Select(dto => dto.ToEntity(to.TradingPair)).ToList();

                    if (trades.Count == 1)
                    {
                        TradeNotificationEventArgs eventArgs = new() { Trade = trades.First(), Type = newTradeType };
                        OnTradeReceived?.Invoke(this, eventArgs);
                        return;
                    }
                    else
                    {
                        TradeSnapshotEventArgs eventArgs = new() { Snapshot = trades };
                        OnTradeSnapshotReceived?.Invoke(this, eventArgs);
                        return;
                    }
                }

                else if (_channels[id] is CandlesChannelObject co)
                {
                    var converter = new CandlesDtoJsonConverter();
                    var dtos = converter.Read(ref jsonReader, typeof(List<CandleDto>), new JsonSerializerOptions());
                    List<Candle> candles = dtos.Select(dto => dto.ToEntity(co.TradingPair)).ToList();

                    if (candles.Count == 1)
                    {
                        CandleNotificationEventArgs eventArgs = new() { Candle = candles.First() };
                        OnCandleReceived?.Invoke(this, eventArgs);
                        return;
                    }
                    else
                    {
                        CandleSnapshotEventArgs eventArgs = new() { Snapshot = candles };
                        OnCandleSnapshotReceived?.Invoke(this, eventArgs);
                        return;
                    }
                }
            }
        }

        public void Dispose() => DisconnectAsync().Wait();
    }
}
