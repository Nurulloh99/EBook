using System.Globalization;
using System.Net;

namespace EBook.Core.Helpers
{
    public static class CookieHelper
    {
        private static string[] DateTimeFormats = new[] {
            "ddd, dd MMM yyyy HH:mm:ss GMT",
            "ddd, dd-MMM-yy HH:mm:ss GMT",
            "ddd, dd-MMM-yyyy HH:mm:ss GMT"
        };

        public static Cookie ToCookie(string setCookieValue, string fallbackDomain)
        {
            Guard.PropertyNotNullOrEmpty(setCookieValue, nameof(setCookieValue));
            var splitted = setCookieValue.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();
            var c = new Cookie();
            var nameValue = SplitPair(splitted[0]);
            var expires = (DateTime?)null;
            var path = (string)null;
            var domain = (string)null;
            var sameSite = (string)null;
            var maxAge = (int?)null;
            var secure = (bool?)null;
            var httpOnly = (bool?)null;
            for (var i = 1; i < splitted.Count; i++)
            {
                var res = SplitPair(splitted[i]);
                var key = res.Key.ToLower();
                switch (key)
                {
                    case "expires":
                        res.Value.TrySafe(x =>
                            {
                                foreach (var format in DateTimeFormats)
                                {
                                    DateTime dResult;
                                    if (DateTime.TryParseExact(x, format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out dResult))
                                    {
                                        expires = dResult;
                                        break;
                                    }
                                }
                            }
                        );
                        break;
                    case "domain":
                        domain = res.Value;
                        break;
                    case "path":
                        path = res.Value;
                        break;
                    case "samesite":
                        sameSite = res.Value;
                        break;
                    case "max-age":
                        maxAge = res.Value.AsInt();
                        break;
                    case "httponly":
                        httpOnly = true;
                        break;
                    case "secure":
                        secure = true;
                        break;
                    default:
                        break;
                }
            }

            if (maxAge.HasValue)
            {
                expires = DateTime.UtcNow.AddSeconds(maxAge.Value);
            }
            c.Name = nameValue.Key;
            c.Value = nameValue.Value;
            c.Expires = expires ?? DateTime.UtcNow.AddDays(365);
            path.With(x => c.Path = x);
            c.Domain = domain ?? fallbackDomain;
            secure.With(x => c.Secure = x.Value);
            httpOnly.With(x => c.HttpOnly = x.Value);
            return c;
        }

        private static KeyValuePair<string, string> SplitPair(string pair)
        {
            var index = pair.IndexOf('=');
            if (index == -1)
            {
                return new KeyValuePair<string, string>(pair, null);
            }
            var key = pair.Substring(0, index);
            var value = pair.Substring(index + 1);
            return new KeyValuePair<string, string>(key, value);
        }
    }
}
