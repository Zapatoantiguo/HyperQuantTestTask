using HyperQuantTestTask.BitfinexLib.Converters;
using HyperQuantTestTask.BitfinexLib.Dto;
using HyperQuantTestTask.BitfinexLib.Enums;
using HyperQuantTestTask.BitfinexLib.Extensions;
using HyperQuantTestTask.BitfinexLib.Model;
using HyperQuantTestTask.BitfinexLib.QueryObjects;
using System.Text.Json;

namespace HyperQuantTestTask.BitfinexLib
{
    public class BitfinexRestApiClient
    {
        HttpClient _httpClient;
        readonly static string baseAddress = "https://api-pub.bitfinex.com/v2/";

        public BitfinexRestApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Ticker?> GetTickerAsync(string pair)
        {
            var queryParams = new TickerQueryParamsObject(pair);
            string url = baseAddress + queryParams.ToUrlPart();

            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new TickerDtoJsonConverter());

            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<TickerDto?>(json, jsonOptions);

            if (dto == null) return null;

            return dto.ToEntity(pair);
        }

        public async Task<IEnumerable<Trade>?> GetTradesAsync(string pair, int limit = 125, 
            SortQueryParam sort = SortQueryParam.Descending,
            DateTimeOffset? start = null, DateTimeOffset? end = null)
        {
            var queryParams = new TradesQueryParamObject(pair, limit, sort, start?.ToUnixTimeMilliseconds(), 
                end?.ToUnixTimeMilliseconds());
            string url = baseAddress + queryParams.ToUrlPart();

            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new TradesDtoJsonConverter());

            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var dtos = JsonSerializer.Deserialize<List<TradeDto>>(json, jsonOptions);

            return dtos.Select(dto => dto.ToEntity(pair));
        }

        public async Task<IEnumerable<Candle>?> GetCandlesAsync(string pair, int limit = 100,
            SortQueryParam sort = SortQueryParam.Descending,
            DateTimeOffset? start = null, DateTimeOffset? end = null,
            BitfinexTimeFrame timeFrame = BitfinexTimeFrame._1m)
        {
            var queryParams = new CandlesQueryParamsObject(pair, limit, sort, start?.ToUnixTimeMilliseconds(), 
                end?.ToUnixTimeMilliseconds(), timeFrame);

            string url = baseAddress + queryParams.ToUrlPart();

            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new CandlesDtoJsonConverter());

            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var dtos = JsonSerializer.Deserialize<List<CandleDto>>(json, jsonOptions);

            return dtos.Select(dto => dto.ToEntity(pair));
        }
    }

    
}
