using ExchangeLibrary.Binance;
using ExchangeLibrary.Binance.Client;
using ExchangeLibrary.Binance.Client.Impl;
using ExchangeLibrary.Binance.DTOs.Marketdata;
using ExchangeLibrary.Binance.EndpointSenders;
using ExchangeLibrary.Binance.EndpointSenders.Impl;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Xunit;

namespace ExchangeLibraryTests.BinanceTests.EndpointSenders
{
    /// <summary>
    ///     �����, ����������� <see cref="MarketdataSender"/>
    /// </summary>
    public class MarketdataSenderTest
    {
        #region Fields

        /// <summary>
        ///     ��������� ��������� ���������� ������� 24�� ��������� ����
        /// </summary>
        private static readonly List<DayPriceChangeDto> _expectedDayPriceChange = new List<DayPriceChangeDto>
        {
            new DayPriceChangeDto
            {
                Symbol = "BNBBTC",
                PriceChange = -94.99999800,
                PriceChangePercent = -95.960,
                WeightedAvgPrice = 0.29628482,
                PrevClosePrice = 0.10002000,
                LastPrice = 4.00000200,
                LastQty = 200.00000000,
                BidPrice = 4.00000000,
                BidQty = 100.00000000,
                AskPrice = 4.00000200,
                AskQty = 100.00000000,
                OpenPrice = 99.00000000,
                HighPrice = 100.00000000,
                LowPrice = 0.10000000,
                Volume = 8913.30000000,
                QuoteVolume = 15.30000000,
                OpenTimeUnix = 1499783499040,
                CloseTimeUnix = 1499869899040,
                FirstId = 28385,
                LastId = 28460,
                Count = 76,
            }
        };

        #endregion

        #region Data

        /// <summary>
        ///     ���� � ������ � ������� ��� �������� ��� ������� 24� �������� ��������� ����
        /// </summary>
        public static IEnumerable<object[]> DayPriceChangeData =>
        new List<object[]>
        {
            /// ���� ��� ������� ���������� � ����
            new object[]
            {
                "BNBBTC",
                "../../../BinanceTests/Jsons/Marketdata/DAY_PRICE_CHANGE_SYMBOL.json",
               _expectedDayPriceChange
            },

            /// ���� ��� ������� ���������� � ���� �����
            new object[]
            {
                null,
                "../../../BinanceTests/Jsons/Marketdata/DAY_PRICE_CHANGE_SYMBOL_IS_NULL.json",
                _expectedDayPriceChange
            },

            /// ���� ��� ������� ���������� � ���� �����
            new object[]
            {
                "",
                "../../../BinanceTests/Jsons/Marketdata/DAY_PRICE_CHANGE_SYMBOL_IS_NULL.json",
                _expectedDayPriceChange
            },
        };

        /// <summary>
        ///     ���� � ������ � ������� ��� �������� ��� ������� ��������� ���� ����/���
        /// </summary>
        public static IEnumerable<object[]> SymbolPriceTickerData =>
        new List<object[]>
        {
            /// ���� ��� ������� ���������� � ����
            new object[]
            {
                "LTCBTC",
                "../../../BinanceTests/Jsons/Marketdata/SYMBOL_PRICE_TICKER.json",
                new List<SymbolPriceTickerDto>
                {
                    new SymbolPriceTickerDto
                    {
                        Symbol = "LTCBTC",
                        Price = 4.00000200
                    }
                }
            },

            /// ���� ��� ������� ���������� � ���� �����
            new object[]
            {
                null,
                "../../../BinanceTests/Jsons/Marketdata/SYMBOL_PRICE_TICKERS.json",
                new List<SymbolPriceTickerDto>
                {
                    new SymbolPriceTickerDto
                    {
                        Symbol = "LTCBTC",
                        Price = 4.00000200
                    },
                    new SymbolPriceTickerDto
                    {
                        Symbol = "ETHBTC",
                        Price = 0.07946600
                    },
                }
            },

            /// ���� ��� ������� ���������� � ���� �����
            new object[]
            {
                "",
                "../../../BinanceTests/Jsons/Marketdata/SYMBOL_PRICE_TICKERS.json",
                new List<SymbolPriceTickerDto>
                {
                    new SymbolPriceTickerDto
                    {
                        Symbol = "LTCBTC",
                        Price = 4.00000200
                    },
                    new SymbolPriceTickerDto
                    {
                        Symbol = "ETHBTC",
                        Price = 0.07946600
                    },
                }
            },
        };

        /// <summary>
        ///     ���� � ������ � ������� ��� �������� ��� ������� ��������� ���� ����/���
        /// </summary>
        public static IEnumerable<object[]> SymbolOrderBookTickerData =>
        new List<object[]>
        {
            /// ���� ��� ������� ���������� � ����
            new object[]
            {
                "LTCBTC",
                "../../../BinanceTests/Jsons/Marketdata/SYMBOL_ORDER_BOOK_TICKER.json",
                new List<SymbolOrderBookTickerDto>
                {
                    new SymbolOrderBookTickerDto
                    {
                        Symbol = "LTCBTC",
                        BidPrice = 4.00000000,
                        BidQty = 431.00000000,
                        AskPrice = 4.00000200,
                        AskQty = 9.00000000,
                    }
                }
            },

            /// ���� ��� ������� ���������� � ���� �����
            new object[]
            {
                null,
                "../../../BinanceTests/Jsons/Marketdata/SYMBOL_ORDER_BOOK_TICKERS.json",
                new List<SymbolOrderBookTickerDto>
                {
                    new SymbolOrderBookTickerDto
                    {
                        Symbol = "LTCBTC",
                        BidPrice = 4.00000000,
                        BidQty = 431.00000000,
                        AskPrice = 4.00000200,
                        AskQty = 9.00000000,
                    },
                    new SymbolOrderBookTickerDto
                    {
                        Symbol = "ETHBTC",
                        BidPrice = 0.07946700,
                        BidQty = 9.00000000,
                        AskPrice = 100000.00000000,
                        AskQty = 1000.00000000,
                    },
                }
            },

            /// ���� ��� ������� ���������� � ���� �����
            new object[]
            {
                "",
                "../../../BinanceTests/Jsons/Marketdata/SYMBOL_ORDER_BOOK_TICKERS.json",
                new List<SymbolOrderBookTickerDto>
                {
                    new SymbolOrderBookTickerDto
                    {
                        Symbol = "LTCBTC",
                        BidPrice = 4.00000000,
                        BidQty = 431.00000000,
                        AskPrice = 4.00000200,
                        AskQty = 9.00000000,
                    },
                    new SymbolOrderBookTickerDto
                    {
                        Symbol = "ETHBTC",
                        BidPrice = 0.07946700,
                        BidQty = 9.00000000,
                        AskPrice = 100000.00000000,
                        AskQty = 1000.00000000,
                    },
                }
            },
        };

        #endregion

        #region Public methods

        /// <summary>
        ///     ���� ������� ������ ������� ��� ���������� ������
        /// </summary>
        [Fact(DisplayName = "Test requesting a list of orders for a specific coin")]
        public async Task GetOrderBookAsyncTest()
        {
            var filePath = "../../../BinanceTests/Jsons/Marketdata/ORDER_BOOK.json";
            using var client = CreateMockHttpClient(BinanceEndpoints.ORDER_BOOK, filePath);
            IBinanceClient binanceClient = new BinanceClient(client, "", "");
            IMarketdataSender marketdataSender = new MarketdataSender(binanceClient);

            // Act
            var result = await marketdataSender.GetOrderBookAsync("", cancellationToken: CancellationToken.None);

            Assert.Single(result.Bids);
            Assert.Single(result.Asks);
            Assert.Equal(4.00000000, result.Bids[0].Price);
            Assert.Equal(431.00000000, result.Bids[0].Qty);
            Assert.Equal(4.00000200, result.Asks[0].Price);
            Assert.Equal(12.00000000, result.Asks[0].Qty);
        }

        /// <summary>
        ///     ���� ������� ������ �������� ������
        /// </summary>
        [Fact(DisplayName = "Test recent trades list query")]
        public async Task GetRecentTradesAsyncTest()
        {
            var filePath = "../../../BinanceTests/Jsons/Marketdata/RECENT_TRADES.json";
            using var client = CreateMockHttpClient(BinanceEndpoints.RECENT_TRADES, filePath);
            IBinanceClient binanceClient = new BinanceClient(client, "", "");
            IMarketdataSender marketdataSender = new MarketdataSender(binanceClient);

            // Act
            var result = (await marketdataSender.GetRecentTradesAsync("", cancellationToken: CancellationToken.None)).ToList();

            Assert.Single(result);
            Assert.Equal(28457, result[0].Id);
            Assert.Equal(4.00000100, result[0].Price);
            Assert.Equal(12.00000000, result[0].Qty);
            Assert.Equal(48.000012, result[0].QuoteQty);
            Assert.Equal(1499865549590, result[0].TimeUnix);
            Assert.True(result[0].IsBuyerMaker);
            Assert.True(result[0].IsBestMatch);
        }

        /// <summary>
        ///     ���� ������� ������ ������������ ������
        /// </summary>
        [Fact(DisplayName = "Test the request for a list of historical trades")]
        public async Task GetOldTradesAsyncTest()
        {
            var filePath = "../../../BinanceTests/Jsons/Marketdata/OLD_TRADES.json";
            using var client = CreateMockHttpClient(BinanceEndpoints.OLD_TRADES, filePath);
            IBinanceClient binanceClient = new BinanceClient(client, "", "");
            IMarketdataSender marketdataSender = new MarketdataSender(binanceClient);

            // Act
            var result = (await marketdataSender.GetOldTradesAsync("", 1000, cancellationToken: CancellationToken.None)).ToList();

            Assert.Single(result);
            Assert.Equal(28457, result[0].Id);
            Assert.Equal(4.00000100, result[0].Price);
            Assert.Equal(12.00000000, result[0].Qty);
            Assert.Equal(48.000012, result[0].QuoteQty);
            Assert.Equal(1499865549590, result[0].TimeUnix);
            Assert.True(result[0].IsBuyerMaker);
            Assert.True(result[0].IsBestMatch);
        }

        /// <summary>
        ///     ���� ������� ������ ������ �� ������
        /// </summary>
        [Fact(DisplayName = "Test request for a list of candlesticks by coin")]
        public async Task GetCandleStickAsyncTest()
        {
            var filePath = "../../../BinanceTests/Jsons/Marketdata/CANDLESTICK_DATA.json";
            using var client = CreateMockHttpClient(BinanceEndpoints.CANDLESTICK_DATA, filePath);
            IBinanceClient binanceClient = new BinanceClient(client, "", "");
            IMarketdataSender marketdataSender = new MarketdataSender(binanceClient);

            // Act
            var result = (await marketdataSender.GetCandleStickAsync("", Common.Enums.CandleStickIntervalType.OneMinute, cancellationToken: CancellationToken.None)).ToList();

            Assert.Single(result);
            Assert.Equal(1499040000000, result[0].OpenTimeUnix);
            Assert.Equal(0.01634790, result[0].OpenPrice);
            Assert.Equal(0.80000000, result[0].MaxPrice);
            Assert.Equal(0.01575800, result[0].MinPrice);
            Assert.Equal(0.01577100, result[0].ClosePrice);
            Assert.Equal(148976.11427815, result[0].Volume);
            Assert.Equal(1499644799999, result[0].CloseTimeUnix);
            Assert.Equal(2434.19055334, result[0].QuoteAssetVolume);
            Assert.Equal(308, result[0].TradesNumber);
            Assert.Equal(1756.87402397, result[0].BasePurchaseVolume);
            Assert.Equal(28.46694368, result[0].QuotePurchaseVolume);
            Assert.Equal("17928899.62484339", result[0].Ignore);
        }

        /// <summary>
        ///     ���� ������� ������� ������� ���� ����
        /// </summary>
        [Fact(DisplayName = "Test request for the current average price of a pair")]
        public async Task GetAveragePriceAsyncTest()
        {
            var filePath = "../../../BinanceTests/Jsons/Marketdata/AVERAGE_PRICE.json";
            using var client = CreateMockHttpClient(BinanceEndpoints.AVERAGE_PRICE, filePath);
            IBinanceClient binanceClient = new BinanceClient(client, "", "");
            IMarketdataSender marketdataSender = new MarketdataSender(binanceClient);

            // Act
            var result = await marketdataSender.GetAveragePriceAsync("", cancellationToken: CancellationToken.None);

            Assert.Equal(5, result.Mins);
            Assert.Equal(9.35751834, result.AveragePrice);
        }

        /// <summary>
        ///     ���� ������� 24� �������� ��������� ���� ����
        /// </summary>
        [Theory(DisplayName = "Test request for a 24-hour change in the price of a pair")]
        [MemberData(nameof(DayPriceChangeData))]
        public async Task GetDayPriceChangeAsyncTest(string symbol, string filePath, List<DayPriceChangeDto> expectedDtos)
        {
            using var client = CreateMockHttpClient(BinanceEndpoints.DAY_PRICE_CHANGE, filePath);
            IBinanceClient binanceClient = new BinanceClient(client, "", "");
            IMarketdataSender marketdataSender = new MarketdataSender(binanceClient);

            // Act
            var result = (await marketdataSender.GetDayPriceChangeAsync(symbol, cancellationToken: CancellationToken.None)).ToList();

            for (var i = 0; i < result.Count; i++)
            {
                var dto = expectedDtos[i];
                var actual = result[i];
                var properties = dto.GetType().GetProperties();
                foreach (var property in properties)
                {
                    Assert.Equal(property.GetValue(dto), property.GetValue(actual));
                }
            }
        }

        /// <summary>
        ///     ���� ������� ��������� ���� ����/���
        /// </summary>
        [Theory(DisplayName = "Test requesting the last price of a pair/pairs")]
        [MemberData(nameof(SymbolPriceTickerData))]
        public async Task GetSymbolPriceTickerAsync(string symbol, string filePath, List<SymbolPriceTickerDto> expectedDtos)
        {
            using var client = CreateMockHttpClient(BinanceEndpoints.SYMBOL_PRICE_TICKER, filePath);
            IBinanceClient binanceClient = new BinanceClient(client, "", "");
            IMarketdataSender marketdataSender = new MarketdataSender(binanceClient);

            // Act
            var result = (await marketdataSender.GetSymbolPriceTickerAsync(symbol, cancellationToken: CancellationToken.None)).ToList();

            for (var i = 0; i < result.Count; i++)
            {
                var dto = expectedDtos[i];
                var actual = result[i];
                var properties = dto.GetType().GetProperties();
                foreach (var property in properties)
                {
                    Assert.Equal(property.GetValue(dto), property.GetValue(actual));
                }
            }
        }

        /// <summary>
        ///     ���� ������� ������ ����/���������� � ������� ��� ������� ��� ��������
        /// </summary>
        [Theory(DisplayName = "Test requesting the best price/quantity in the order book for a symbol or symbols")]
        [MemberData(nameof(SymbolOrderBookTickerData))]
        public async Task GetSymbolOrderBookTickerAsyncAsync(string symbol, string filePath, List<SymbolOrderBookTickerDto> expectedDtos)
        {
            using var client = CreateMockHttpClient(BinanceEndpoints.SYMBOL_ORDER_BOOK_TICKER, filePath);
            IBinanceClient binanceClient = new BinanceClient(client, "", "");
            IMarketdataSender marketdataSender = new MarketdataSender(binanceClient);

            // Act
            var result = (await marketdataSender.GetSymbolOrderBookTickerAsync(symbol, cancellationToken: CancellationToken.None)).ToList();

            for (var i = 0; i < result.Count; i++)
            {
                var dto = expectedDtos[i];
                var actual = result[i];
                var properties = dto.GetType().GetProperties();
                foreach (var property in properties)
                {
                    Assert.Equal(property.GetValue(dto), property.GetValue(actual));
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        ///     ������� ��� HttpClient
        /// </summary>
        /// <param name="filePath"> ���� � ����� � json-response </param>
        /// <returns></returns>
        private HttpClient CreateMockHttpClient(string url, string filePath)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(basePath, filePath);
            var json = File.ReadAllText(path);
            using var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(url).Respond("application/json", json);

            return new HttpClient(mockHttp);
        }

        #endregion
    }
}
