﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace ExchangeLibrary.Binance.Client.Impl
{
    /// <summary>
    ///     Клиент Binance для разделов REST API.
    /// </summary>
    internal class BinanceClient : IBinanceClient
    {
        #region Fields

        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        #endregion

        #region .ctor

        /// <inheritdoc cref="BinanceClient"/>
        public BinanceClient(HttpClient httpClient, string baseUrl, string apiKey, string apiSecret)
        {
            _client = httpClient;
            _baseUrl = baseUrl;
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        #endregion

        public async Task<T> SendPublicAsync<T>(string requestUri, HttpMethod httpMethod, Dictionary<string, object> query = null, object content = null)
        {
            if (!(query is null))
            {
                StringBuilder queryStringBuilder = this.BuildQueryString(query, new StringBuilder());

                if (queryStringBuilder.Length > 0)
                {
                    requestUri += "?" + queryStringBuilder.ToString();
                }
            }

            return await this.SendAsync<T>(requestUri, httpMethod, content);
        }

        public async Task<T> SendSignedAsync<T>(string requestUri, HttpMethod httpMethod, Dictionary<string, object> query = null, object content = null)
        {
            StringBuilder queryStringBuilder = new StringBuilder();

            if (!(query is null))
            {
                queryStringBuilder = this.BuildQueryString(query, queryStringBuilder);
            }

            string signature = Sign(queryStringBuilder.ToString(), this._apiSecret);

            if (queryStringBuilder.Length > 0)
            {
                queryStringBuilder.Append("&");
            }

            queryStringBuilder.Append("signature=").Append(signature);

            requestUri += "?" + queryStringBuilder.ToString();

            return await this.SendAsync<T>(requestUri, httpMethod, content);
        }

        private static string Sign(string source, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (HMACSHA256 hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);

                byte[] hash = hmacsha256.ComputeHash(sourceBytes);

                return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            }
        }

        private StringBuilder BuildQueryString(Dictionary<string, object> queryParameters, StringBuilder builder)
        {
            foreach (KeyValuePair<string, object> queryParameter in queryParameters)
            {
                if (!string.IsNullOrWhiteSpace(queryParameter.Value?.ToString()))
                {
                    if (builder.Length > 0)
                    {
                        builder.Append("&");
                    }

                    builder
                        .Append(queryParameter.Key)
                        .Append("=")
                        .Append(HttpUtility.UrlEncode(queryParameter.Value.ToString()));
                }
            }

            return builder;
        }

        private async Task<T> SendAsync<T>(string requestUri, HttpMethod httpMethod, object content = null)
        {
            using (var request = new HttpRequestMessage(httpMethod, this._baseUrl + requestUri))
            {
                if (!(content is null))
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
                }

                if (!(this._apiKey is null))
                {
                    request.Headers.Add("X-MBX-APIKEY", this._apiKey);
                }

                HttpResponseMessage response = await this._client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent responseContent = response.Content)
                    {
                        string jsonString = await responseContent.ReadAsStringAsync();

                        if (typeof(T) == typeof(string))
                        {
                            return (T)(object)jsonString;
                        }
                        else
                        {
                            try
                            {
                                T data = JsonConvert.DeserializeObject<T>(jsonString);

                                return data;
                            }
                            catch (JsonReaderException ex)
                            {
                                var clientException = new BinanceClientException($"Failed to map server response from '${requestUri}' to given type", -1, ex);

                                clientException.StatusCode = (int)response.StatusCode;
                                clientException.Headers = response.Headers.ToDictionary(a => a.Key, a => a.Value);

                                throw clientException;
                            }
                        }
                    }
                }
                else
                {
                    using (HttpContent responseContent = response.Content)
                    {
                        BinanceHttpException httpException = null;
                        string contentString = await responseContent.ReadAsStringAsync();
                        int statusCode = (int)response.StatusCode;
                        if (400 <= statusCode && statusCode < 500)
                        {
                            if (string.IsNullOrWhiteSpace(contentString))
                            {
                                httpException = new BinanceClientException("Unsuccessful response with no content", -1);
                            }
                            else
                            {
                                try
                                {
                                    httpException = JsonConvert.DeserializeObject<BinanceClientException>(contentString);
                                }
                                catch (JsonReaderException ex)
                                {
                                    httpException = new BinanceClientException(contentString, -1, ex);
                                }
                            }
                        }
                        else
                        {
                            httpException = new BinanceServerException(contentString);
                        }

                        httpException.StatusCode = statusCode;
                        httpException.Headers = response.Headers.ToDictionary(a => a.Key, a => a.Value);

                        throw httpException;
                    }
                }
            }
        }
    }
}
