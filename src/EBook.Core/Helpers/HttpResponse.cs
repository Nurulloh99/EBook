using System.Net;
namespace EBook.Core.Helpers
{
    public class HttpResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<KeyValuePair<string, string>> ResponseHeaders { get; set; }
        public Stream ResponseStream { get; set; }
        public string Domain { get; }
        public bool ErrorOccurred { get; set; }
        public HttpResponse() { }
        public HttpResponse(
            HttpStatusCode statusCode,
            List<KeyValuePair<string, string>> responseHeaders,
            Stream responseStream,
            string domain
        )
        {
            StatusCode = statusCode;
            ResponseHeaders = responseHeaders;
            ResponseStream = responseStream;
            Domain = domain;
        }

        public string GetLocation()
        {
            return ResponseHeaders
                .FirstOrDefault(x => x.Key.Equals("location", StringComparison.OrdinalIgnoreCase))
                .With(x => x.Value);
        }

        public Cookie[] GetSetCookies()
        {
            return ResponseHeaders
                    .Where(x => x.Key.Equals("set-cookie", StringComparison.OrdinalIgnoreCase))
                    .Select(x => CookieHelper.ToCookie(x.Value, Domain))
                    .ToArray();
        }
    }
}