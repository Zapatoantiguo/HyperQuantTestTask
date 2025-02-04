using HyperQuantTestTask.BitfinexLib.Dto;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace HyperQuantTestTask.BitfinexLib.Converters
{
    public class CandlesDtoJsonConverter : JsonConverter<List<CandleDto>>
    {
        public override List<CandleDto> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = new List<CandleDto>();
            CandleDto? currentCandle = null;
            // на старте указатель на токене [ - начало массива массивов

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartArray:
                        {
                            currentCandle = new CandleDto();
                            reader.Read();  // Millisecond epoch timestamp
                            currentCandle.Timestamp = reader.GetInt64();
                            reader.Read();  // 	First execution during the time frame
                            currentCandle.OpenPrice = reader.GetInt64();
                            reader.Read();  // Last execution during the time frame
                            currentCandle.ClosePrice = reader.GetInt64();
                            reader.Read();  // Highest execution during the time frame
                            currentCandle.HighPrice = reader.GetInt64();
                            reader.Read();  // Lowest execution during the timeframe
                            currentCandle.LowPrice = reader.GetInt64();
                            reader.Read();  // Quantity of symbol traded within the timeframe
                            currentCandle.Volume = reader.GetDecimal();

                            result.Add(currentCandle);
                            break;
                        }
                }
            }

            return result;
        }


        public override void Write(Utf8JsonWriter writer, List<CandleDto> candleDtos, JsonSerializerOptions options) =>
                throw new NotImplementedException();
    }
}
