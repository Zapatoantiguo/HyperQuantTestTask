using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using HyperQuantTestTask.BitfinexLib.Dto;

namespace HyperQuantTestTask.BitfinexLib.Converters
{
    public class TickerDtoJsonConverter : JsonConverter<TickerDto?>
    {
        public override TickerDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = new TickerDto();
            // на старте указатель на токене [
            reader.Read();  // BID

            if (reader.TokenType == JsonTokenType.EndArray) // пустой объект
                return null;

            result.Bid = reader.GetDecimal();
            reader.Read();  // BID_SIZE
            result.BidSize = reader.GetDecimal();
            reader.Read();  // ASK
            result.Ask = reader.GetDecimal();
            reader.Read();  // ASK_SIZE
            result.AskSize = reader.GetDecimal();
            reader.Read();  // DAILY_CHANGE
            result.DailyChange = reader.GetDecimal();
            reader.Read();  // DAILY_CHANGE_RELATIVE
            result.DailyChangeRelative = reader.GetDecimal();
            reader.Read();  // LAST_PRICE
            result.LastPrice = reader.GetDecimal();
            reader.Read();  // VOLUME
            result.Volume = reader.GetDecimal();
            reader.Read();  // HIGH
            result.High = reader.GetDecimal();
            reader.Read();  // LOW
            result.Low = reader.GetDecimal();

            reader.Read(); // ]

            return result;
        }


        public override void Write(Utf8JsonWriter writer, TickerDto tickerDto, JsonSerializerOptions options) =>
                throw new NotImplementedException();
    }
}
