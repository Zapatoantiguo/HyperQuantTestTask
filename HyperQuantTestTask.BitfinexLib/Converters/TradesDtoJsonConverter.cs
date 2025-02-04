﻿using HyperQuantTestTask.BitfinexLib.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Converters
{
    public class TradesDtoJsonConverter : JsonConverter<List<TradeDto>>
    {
        public override List<TradeDto> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = new List<TradeDto>();
            TradeDto? currentTrade = null;
            // на старте указатель на токене [ - начало массива массивов

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartArray:
                        {
                            currentTrade = new TradeDto();

                            reader.Read();  // ID of the trade
                            currentTrade.Id = reader.GetInt64();
                            reader.Read();  // Millisecond epoch timestamp
                            currentTrade.Timestamp = reader.GetInt64();
                            reader.Read();  // How much was bought (positive) or sold (negative)
                            currentTrade.Amount = reader.GetDecimal();
                            reader.Read();  // Price at which the trade was executed
                            currentTrade.Price = reader.GetDecimal();

                            result.Add(currentTrade);
                            break;
                        }
                }
            }

            return result;
        }


        public override void Write(Utf8JsonWriter writer, List<TradeDto> tradeDtos, JsonSerializerOptions options) =>
                throw new NotImplementedException();
    }
}
