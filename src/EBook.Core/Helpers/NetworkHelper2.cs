using System.Net;
using System.Text;
using System.Web;
namespace EBook.Core.Helpers
{
    public class NetworkHelper2
    {
        private readonly string UserAgent;

        public NetworkHelper2(string userAgent)
        {
            Guard.PropertyNotNullOrEmpty(userAgent, "userAgent");
            UserAgent = userAgent;
        }

        public HttpResponse MakeRemoteRequest(HttpRequest info)
        {
            Guard.ParamNotNull(info, "info");
            info.Validate();
            var domain = info.GetDomain();
            var req = GetRequest(info);

            var length = SetupBody(info, req);
            if ((info.HttpVerb == HttpVerb.Put || info.HttpVerb == HttpVerb.Post) && req.ContentLength == -1)
                req.ContentLength = length ?? 0;
            try
            {
                return ProcessResponse((HttpWebResponse)req.GetResponse(), domain);
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    throw;

                var code = (int)((HttpWebResponse)ex.Response).StatusCode;
                if (!(code >= 300 && code < 400) && !info.ProceedOnError)
                {
                    throw;
                }
                var result = ProcessResponse((HttpWebResponse)ex.Response, domain);
                result.ErrorOccurred = !(code >= 300 && code < 400);
                return result;
            }
        }

        public Task<HttpResponse> MakeRemoteRequestAsync(HttpRequest info)
        {
            return Task.Factory.StartNew(() => MakeRemoteRequest(info));
        }

        public HttpResponse MakeRemoteRequest(string url)
        {
            return MakeRemoteRequest(url, HttpVerb.Get);
        }

        public Task<HttpResponse> MakeRemoteRequestAsync(string url)
        {
            return Task.Factory.StartNew(() => MakeRemoteRequest(url));
        }

        public HttpResponse MakeRemoteRequest(string url, HttpVerb verb)
        {
            return MakeRemoteRequest(url, verb, null);
        }

        public Task<HttpResponse> MakeRemoteRequestAsync(string url, HttpVerb verb)
        {
            return Task.Factory.StartNew(() => MakeRemoteRequest(url, verb));
        }

        public HttpResponse MakeRemoteRequest(string url, HttpVerb verb, string body)
        {
            return MakeRemoteRequest(new HttpRequest
            {
                Url = url,
                HttpVerb = verb,
                PostBody = body
            });
        }

        public Task<HttpResponse> MakeRemoteRequestAsync(string url, HttpVerb verb, string body)
        {
            return Task.Factory.StartNew(() => MakeRemoteRequest(url, verb, body));
        }

        private static HttpResponse ProcessResponse(HttpWebResponse response, string domain)
        {
            var res = (Stream)null;
            using (var rs = response.GetResponseStream())
                res = rs.CopyToMemory();
            if (res.Position != 0)
                res.Position = 0;
            var headers = new List<KeyValuePair<string, string>>();
            foreach (var key in response.Headers.AllKeys)
            {
                var values = response.Headers.GetValues(key);
                headers.AddRange(values.Select(x => new KeyValuePair<string, string>(key, x)));
            }
            //var headers = response.Headers.AllKeys.Select(x => new KeyValuePair<string, string>(x, response.Headers[x])).ToList();
            return new HttpResponse(response.StatusCode, headers, res, domain);
        }

        private static long? SetupBody(HttpRequest info, HttpWebRequest reqObject)
        {
            if (info.MultipartPostParams.Any())
                return FillMultipart(info, reqObject);
            if (info.PostBinaryBody != null && info.PostBinaryBody.Any())
            {
                if (reqObject.ContentType == null)
                    reqObject.ContentType = HttpRequest.ContentTypeFormUrlEncoded;
                using (var rs = reqObject.GetRequestStream())
                {
                    rs.Write(info.PostBinaryBody);
                    return info.PostBinaryBody.LongLength;
                }
            }
            if (!string.IsNullOrEmpty(info.PostBody))
            {
                if (reqObject.ContentType == null)
                    reqObject.ContentType = HttpRequest.ContentTypeFormUrlEncoded;
                using (var rs = reqObject.GetRequestStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(info.PostBody);
                    rs.Write(bytes);
                    return bytes.LongLength;
                }
            }
            if (info.PostParams.Any())
            {
                if (reqObject.ContentType == null)
                    reqObject.ContentType = HttpRequest.ContentTypeFormUrlEncoded;
                using (var rs = reqObject.GetRequestStream())
                {
                    var result = info.PostParams.Aggregate(new StringBuilder(), (sb, x) => sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(x.Key), HttpUtility.UrlEncode(x.Value)), x => x.Length > 0 ? x.RemoveLast().ToString() : x.ToString());
                    var bytes = Encoding.UTF8.GetBytes(result);
                    rs.Write(bytes);
                    return bytes.LongLength;
                }
            }
            return null;
        }

        /// <summary>
        /// Populates the body of the request and returns the actual content length
        /// </summary>
        /// <param name="info"></param>
        /// <param name="reqObject"></param>
        /// <returns></returns>
        private static long FillMultipart(HttpRequest info, HttpWebRequest reqObject)
        {
            var ms = new MemoryStream();
            var @params = info.MultipartPostParams;
            var boundary = string.Format("----------{0:N}", Guid.NewGuid());
            reqObject.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            var needsNewLine = false;
            foreach (var item in @params)
            {
                if (needsNewLine)
                    ms.WriteText("\r\n");
                needsNewLine = true;
                var header = (string)null;
                if (item.FileName != null && item.ContentType != null)
                    header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n", boundary, item.ParamName, item.FileName, item.ContentType);
                else
                    header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n", boundary, item.ParamName);
                ms.WriteText(header);
                ms.Write(item.Value);
            }
            ms.WriteText(string.Format("\r\n--{0}--\r\n", boundary));
            ms.Position = 0;
            using (var rs = reqObject.GetRequestStream())
                ms.CopyTo(rs);
            return ms.Length;
        }
        private HttpWebRequest GetRequest(HttpRequest info)
        {
            var url = info.GetUrl();
            var client = new HttpClient();
            var req = (HttpWebRequest)WebRequest.Create(url);

            var headers = info.GetAllHeaders();
            var typedHeaders = headers.Keys.OfType<HttpRequestHeader>().ToDictionary(x => x, x => headers[x]);
            var stringHeaders = headers.Keys.OfType<string>().ToDictionary(x => x, x => headers[x]);
            req.Method = info.HttpVerb.ToString().ToUpper();
            req.UserAgent = info.UserAgent ?? UserAgent;
            if (info.AllowAutoRedirect.HasValue)
                req.AllowAutoRedirect = info.AllowAutoRedirect.Value;
            info.Timeout.Try(x =>
            {
                if (x.Value > TimeSpan.Zero)
                    req.Timeout = (int)x.Value.TotalMilliseconds;
            });

            if (!string.IsNullOrWhiteSpace(info.UserName))
                req.Headers["Authorization"] = string.Format("Basic {0}", string.Format("{0}:{1}", info.UserName, info.Password).ToByte().ToBase64());
            if (!string.IsNullOrWhiteSpace(info.BearerToken))
                req.Headers["Authorization"] = string.Format("Bearer {0}", info.BearerToken);

            if (info.EmulateAjax)
                req.Headers["X-Requested-With"] = "XMLHttpRequest";

            if (info.Cookies != null && info.Cookies.Any())
            {
                req.CookieContainer = new CookieContainer();
                info.Cookies.ForEach(x => req.CookieContainer.Add(x));
            }

            setUpHeader(HttpRequestHeader.ContentType, x => req.ContentType = x);
            setUpHeader(HttpRequestHeader.Accept, x => req.Accept = x);
            setUpHeader(HttpRequestHeader.Referer, x => req.Referer = x);
            setUpHeader(HttpRequestHeader.Connection, x => req.Connection = x);

            foreach (var header in typedHeaders)
                req.Headers.Add(header.Key, header.Value);
            foreach (var header in stringHeaders)
                req.Headers.Add(header.Key, header.Value);

            return req;

            void setUpHeader(HttpRequestHeader x, Action<string> y)
            {
                if (typedHeaders.ContainsKey(x))
                {
                    y(typedHeaders[x]);
                    typedHeaders.Remove(x);
                }
            }
        }
    }
}