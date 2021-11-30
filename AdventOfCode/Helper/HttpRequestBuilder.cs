using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Threading.Tasks;

namespace AdventOfCode.Helper
{
    public class HttpRequestBuilder
    {
        private HttpMethod method = null;
        private string requestUri = "";
        private HttpContent content = null;
        private string bearerToken = "";
        private string acceptHeader = "application/json";
        private List<KeyValuePair<string,string>> cookies = new List<KeyValuePair<string,string>>();
        private TimeSpan timeout = new TimeSpan(0, 0, 15);

        public HttpRequestBuilder()
        {

        }

        public HttpRequestBuilder AddMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }

        public HttpRequestBuilder AddRequestUri(string requestUri)
        {
            this.requestUri = requestUri;
            return this;
        }

        public HttpRequestBuilder AddContent(HttpContent content)
        {
            this.content = content;
            return this;
        }

        public HttpRequestBuilder AddBearerToken(string bearerToken)
        {
            this.bearerToken = bearerToken;
            return this;
        }

        public HttpRequestBuilder AddAcceptHeader(string acceptHeader)
        {
            this.acceptHeader = acceptHeader;
            return this;
        }

        public HttpRequestBuilder AddCookie(string cookieName, string cookieValue)
        {
            cookies.Add(new KeyValuePair<string, string>(cookieName, cookieValue));
            return this;
        }

        public HttpRequestBuilder AddTimeout(TimeSpan timeout)
        {
            this.timeout = timeout;
            return this;
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            // Check required arguments
            EnsureArguments();

            // Setup request
            var request = new HttpRequestMessage
            {
                Method = this.method,
                RequestUri = new Uri(this.requestUri)
            };

            if (this.content != null)
                request.Content = this.content;

            if (!string.IsNullOrEmpty(this.bearerToken))
                request.Headers.Authorization =
                  new AuthenticationHeaderValue("Bearer", this.bearerToken);

            request.Headers.Accept.Clear();
            if (!string.IsNullOrEmpty(this.acceptHeader))
                request.Headers.Accept.Add(
                   new MediaTypeWithQualityHeaderValue(this.acceptHeader));

            // Setup client
            var cookieContainer = new CookieContainer();
            foreach(var cookie in cookies)
            {
                cookieContainer.Add(new Cookie(cookie.Key, cookie.Value));    
            }
            using (var httpClientHandler = new HttpClientHandler {CookieContainer = cookieContainer })
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    if (errors.HasFlag(SslPolicyErrors.None))
                        return true;

                    return false;
                };
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.Timeout = this.timeout;
                    return await client.SendAsync(request);
                }
            }
        }

        private void EnsureArguments()
        {
            if (this.method == null)
                throw new ArgumentNullException("Method");

            if (string.IsNullOrEmpty(this.requestUri))
                throw new ArgumentNullException("Request Uri");
        }
    }
}
