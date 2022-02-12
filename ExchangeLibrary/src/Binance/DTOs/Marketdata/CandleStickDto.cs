﻿using System;
using System.Collections.Generic;
using Common.JsonConvertWrapper;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExchangeLibrary.Binance.DTOs.Marketdata
{
    /// <summary>
    ///     Модель свечи для монеты
    /// </summary>
    public class CandleStickDto
    {
        /// <summary>
        ///     Время открытия
        /// </summary>
        public long OpenTimeUnix { get; set; }

        /// <summary>
        ///     Цена открытия
        /// </summary>
        public double OpenPrice { get; set; }

        /// <summary>
        ///     Минимальная цена
        /// </summary>
        public double MinPrice { get; set; }

        /// <summary>
        ///     Максимальная цена
        /// </summary>
        public double MaxPrice { get; set; }

        /// <summary>
        ///     Объем
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        ///     Цена закрытия
        /// </summary>
        public double ClosePrice { get; set; }

        /// <summary>
        ///     Время закрытия
        /// </summary>
        public long CloseTimeUnix { get; set; }

        /// <summary>
        ///     Объем котируемого актива
        /// </summary>
        public double QuoteAssetVolume { get; set; }

        /// <summary>
        ///     Кол-во сделок
        /// </summary>
        public int TradesNumber { get; set; }

        /// <summary>
        ///     Объем базового актива, который купили тейкеры
        /// </summary>
        public double BasePurchaseVolume { get; set; }

        /// <summary>
        ///     Объем актива по котировке тейкера на покупку
        /// </summary>
        public double QuotePurchaseVolume { get; set; }

        /// <summary>
        ///     Устанавливает св-ва для <see cref="CandleStickDto"/>
        /// </summary>
        internal static void SetPropertiesCandleStickDto(ref Utf8JsonReader reader, CandleStickDto result)
        {
            result.OpenTimeUnix = reader.ReadLongAndNext();
            result.OpenPrice = reader.ReadDoubleAndNext();
            result.MaxPrice = reader.ReadDoubleAndNext();
            result.MinPrice = reader.ReadDoubleAndNext();
            result.ClosePrice = reader.ReadDoubleAndNext();
            result.Volume = reader.ReadDoubleAndNext();
            result.CloseTimeUnix = reader.ReadLongAndNext();
            result.QuoteAssetVolume = reader.ReadDoubleAndNext();
            result.TradesNumber = reader.ReadIntAndNext();
            result.BasePurchaseVolume = reader.ReadDoubleAndNext();
            result.QuotePurchaseVolume = reader.ReadDoubleAndNext();
        }
    }

    public class CandleStickDtoEnumerableConverter : JsonConverter<IEnumerable<CandleStickDto>>
    {
        /// <inheritdoc />
        public override IEnumerable<CandleStickDto> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = new List<CandleStickDto>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var newCandleStick = new CandleStickDto();
                    reader.Read();
                    CandleStickDto.SetPropertiesCandleStickDto(ref reader, newCandleStick);

                    result.Add(newCandleStick);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, IEnumerable<CandleStickDto> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///     Нормально конвертирует полученные данные
    /// </summary>
    public class CandleStickDtoConverter : JsonConverter<CandleStickDto>
    {
        /// <inheritdoc />
        public override CandleStickDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = new CandleStickDto();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read();
                    CandleStickDto.SetPropertiesCandleStickDto(ref reader, result);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, CandleStickDto value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
