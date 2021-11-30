using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdventOfCode.Helper
{
    public static class HttpRequestFactory
    {
        public const long TicksPerSecond = 10000000;

        public static async Task<HttpResponseMessage> Get(string bearerToken, string requestUri, bool longTimeout = false)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Get)
                                .AddRequestUri(requestUri);

            if (!string.IsNullOrEmpty(bearerToken))
            {
                builder.AddBearerToken(bearerToken);
            }

            if (longTimeout)
            {
                builder.AddTimeout(new System.TimeSpan(TicksPerSecond * 300));
            }

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Post(
           string bearerToken, string requestUri, object value = null, bool longTimeout = false)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Post)
                                .AddRequestUri(requestUri);

            if (!string.IsNullOrEmpty(bearerToken))
            {
                builder.AddBearerToken(bearerToken);
            }

            if (longTimeout)
            {
                builder.AddTimeout(new System.TimeSpan(TicksPerSecond * 300));
            }

            if (value != null)
                builder.AddContent(new JsonContent(value));

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Put(
           string bearerToken, string requestUri, object value = null, bool longTimeout = false)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Put)
                                .AddRequestUri(requestUri);

            if (!string.IsNullOrEmpty(bearerToken))
            {
                builder.AddBearerToken(bearerToken);
            }

            if (longTimeout)
            {
                builder.AddTimeout(new System.TimeSpan(TicksPerSecond * 300));
            }

            if (value != null)
                builder.AddContent(new JsonContent(value));

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Patch(
           string bearerToken, string requestUri, object value = null, bool longTimeout = false)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(new HttpMethod("PATCH"))
                                .AddRequestUri(requestUri);

            if (!string.IsNullOrEmpty(bearerToken))
            {
                builder.AddBearerToken(bearerToken);
            }

            if (longTimeout)
            {
                builder.AddTimeout(new System.TimeSpan(TicksPerSecond * 300));
            }

            if (value != null)
                builder.AddContent(new JsonContent(value));

            return await builder.SendAsync();
        }

        public static async Task<HttpResponseMessage> Delete(string bearerToken, string requestUri, bool longTimeout = false)
        {
            var builder = new HttpRequestBuilder()
                                .AddMethod(HttpMethod.Delete)
                                .AddRequestUri(requestUri);

            if (!string.IsNullOrEmpty(bearerToken))
            {
                builder.AddBearerToken(bearerToken);
            }

            if (longTimeout)
            {
                builder.AddTimeout(new System.TimeSpan(TicksPerSecond * 300));
            }

            return await builder.SendAsync();
        }

        public static Task Delete(string accessToken, object p)
        {
            throw new NotImplementedException();
        }
    }
}
