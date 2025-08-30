using EBook.Errors;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace EBook.Core.Helpers
{
    public static class HelperExtensions
    {
        /// <summary>
        /// Safer ToString() method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AsString(this object value)
        {
            return value == null ? null : value.ToString();
        }
        public static string AsString(this byte[] value)
        {
            return value.AsString(Encoding.UTF8);
        }
        public static string AsString(this byte[] value, Encoding encoding)
        {
            if (value == null)
                throw null;
            Guard.ParamNotNull(encoding, "encoding");
            return encoding.GetString(value);
        }
        public static string AsString(this object value, string format)
        {
            if (value == null)
                return null;
            return string.Format(string.Format("{{0:{0}}}", format), value);
        }
        public static string AsString(this object value, string format, IFormatProvider formatProvider)
        {
            Guard.ParamNotNull(formatProvider, "formatProvider");
            if (value == null)
                return null;
            return string.Format(formatProvider, string.Format("{{0:{0}}}", format), value);
        }
        /// <summary>
        /// Serializes Exception into XElement
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static XElement AsXmlElement(this Exception ex)
        {
            if (ex == null)
                return null;
            var element = new XElement("Exception");
            if (!string.IsNullOrWhiteSpace(ex.Message))
                element.Add(new XElement("Message", ex.Message));
            if (!string.IsNullOrWhiteSpace(ex.Source))
                element.Add(new XElement("Source", ex.Source));
            if (!string.IsNullOrWhiteSpace(ex.StackTrace))
                element.Add(new XElement("StackTrace", ex.StackTrace));
            if (ex.InnerException != null)
                element.Add(new XElement("InnerException", ex.InnerException.AsXmlElement()));
            return element;
        }
        /// <summary>
        /// Converts this value to int
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns Nullable&lt;int&gt;</returns>
        public static int? AsInt(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var k))
                    return k;
            }
            return default;
        }
        /// <summary>
        /// Converts this value to long
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns Nullable&lt;long&gt;</returns>
        public static long? AsLongInt(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                long k;
                if (long.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out k))
                    return k;
            }
            return default(long?);
        }

        /// <summary>
        /// Converts this value to double
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns Nullable&lt;double&gt;</returns>
        public static Double? AsDouble(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Double k;
                if (Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out k))
                {
                    return k;
                }
            }
            return default(Double?);
        }

        /// <summary>
        /// Converts this value to decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns Nullable&lt;decimal&gt;</returns>
        public static Decimal? AsDecimal(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Decimal k;
                if (Decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out k))
                    return k;
            }
            return default(Decimal?);
        }

        /// <summary>
        /// Converts this value to DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns Nullable&lt;DateTime&gt;</returns>
        public static DateTime? AsDateTime(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                DateTime d;
                if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                    return d;
            }
            return default;
        }

        /// <summary>
        /// Converts this value to bool, if parsing fails, will be returned <paramref name="@default"/>
        /// </summary>
        /// <param name="value">value to convert (true|false|on|off insensetive to case)</param>
        /// <returns>Returns bool representation of the string value</returns>
        public static bool? AsBoolean(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                bool d;
                if (bool.TryParse(value, out d))
                    return d;
                if (string.Equals(value, "on", StringComparison.OrdinalIgnoreCase))
                    return true;
                if (string.Equals(value, "off", StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return default(bool?);
        }

        /// <summary>
        /// Converts the string into the Guid, and returns null if the conversion fails
        /// </summary>
        /// <param name="value">string vaue to convert.</param>
        /// <returns>Conversion result</returns>
        public static Guid? AsGuid(this string value)
        {
            Guid gVal;
            if (Guid.TryParse(value, out gVal))
                return gVal;
            return null;
        }

        /// <summary>
        /// Converts this value to int, if parsing fails, will be returned <paramref name="default"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="default">the default value for fail case</param>
        /// <returns>Returns int</returns>
        public static int AsInt(this string value, int @default)
        {
            if (!string.IsNullOrEmpty(value))
            {
                int k;
                if (int.TryParse(value, out k))
                    return k;
            }
            return @default;
        }

        /// <summary>
        /// Converts this value to long, if parsing fails, will be returned <paramref name="default"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="default">the default value for fail case</param>
        /// <returns>Returns int</returns>
        public static long AsLongInt(this string value, long @default)
        {
            if (!string.IsNullOrEmpty(value))
            {
                long k;
                if (long.TryParse(value, out k))
                    return k;
            }
            return @default;
        }

        /// <summary>
        /// Converts this value to double, if parsing fails, will be returned <paramref name="default"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="default">the default value for fail case</param>
        /// <returns>Returns double</returns>
        public static Double AsDouble(this string value, Double @default)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Double k;
                if (Double.TryParse(value, out k))
                    return k;
            }
            return @default;
        }
        /// <summary>
        /// Converts this value to decimal, if parsing fails, will be returned <paramref name="@default"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="@default">the default value for fail case</param>
        /// <returns>Returns decimal</returns>
        public static Decimal AsDecimal(this string value, Decimal @default)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Decimal k;
                if (Decimal.TryParse(value, out k))
                    return k;
            }
            return @default;
        }
        /// <summary>
        /// Converts this value to DateTime, if parsing fails, will be returned <paramref name="@default"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="@default">the default value for fail case</param>
        /// <returns>Returns DateTime</returns>
        public static DateTime AsDateTime(this string value, DateTime @default)
        {
            if (!string.IsNullOrEmpty(value))
            {
                DateTime d;
                if (DateTime.TryParse(value, out d))
                    return d;
            }
            return @default;
        }
        /// <summary>
        /// Converts this value to bool, if parsing fails, will be returned <paramref name="@default"/>
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="@default">the default value for fail case</param>
        /// <returns>Returns bool</returns>
        public static bool AsBoolean(this string value, bool @default)
        {
            if (!string.IsNullOrEmpty(value))
            {
                bool d;
                if (bool.TryParse(value, out d))
                    return d;
                if (string.Equals(value, "on", StringComparison.OrdinalIgnoreCase))
                    return true;
                if (string.Equals(value, "off", StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return @default;
        }
        /// <summary>
        /// Attemtps to convert the provided string into the Guid and returns the provided value if the conversion fails.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="default">Default value to return if the conversion fails.</param>
        /// <returns></returns>
        public static Guid AsGuid(this string value, Guid @default)
        {
            Guid res;
            if (Guid.TryParse(value, out res))
                return res;
            return @default;
        }

        public static string Format(this string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            return string.Format(format, args);
        }
        public static string ToText(this byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            return Encoding.UTF8.GetString(input);
        }
        public static byte[] ToByte(this string input)
        {
            return input.ToByte(Encoding.UTF8);
        }
        public static byte[] ToByte(this string input, Encoding encoding)
        {
            Guard.ParamNotNull(input, "input");
            Guard.ParamNotNull(encoding, "encoding");
            return encoding.GetBytes(input);
        }
        public static ImageFormatType GetImageFormat(this Stream data)
        {
            Guard.ParamNotNull(data, "data");
            if (!data.CanSeek || !data.CanRead)
                throw new NotSupportedException("This operation is not supported for the streams that cannot be read or written");

            var buffer = new byte[4];
            var current = data.Position;
            data.Position = 0;
            data.Read(buffer, 0, 4);
            data.Position = current;
            return buffer.GetImageFormat();
        }
        public static ImageFormatType GetImageFormat(this byte[] bytes)
        {
            Guard.ParamNotNull(bytes, "bytes");
            // see http://www.mikekunz.com/image_file_header.html  
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormatType.Bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormatType.Gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormatType.Png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormatType.Tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormatType.Tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormatType.Jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormatType.Jpeg;

            return ImageFormatType.Unknown;
        }
        public static bool TryCast<T>(this object obj, out T target)
        {
            try
            {
                target = (T)obj;
                return true;
            }
            catch { }
            try
            {
                target = (T)Convert.ChangeType(obj, typeof(T));
                return true;
            }
            catch { }
            target = default(T);
            return false;
        }
        public static T CastEnum<T>(this object value) where T : struct
        {
            if (value == null)
                throw new InvalidArgumentException("You passed null");
            if (value.GetType() == typeof(int))
                return (T)value;
            if (value.GetType() == typeof(string))
            {
                var intVal = ((string)value).AsInt();
                if (intVal.HasValue)
                    return (T)(object)intVal.Value;
                if (Enum.TryParse((string)value, true, out T tmpVal))
                {
                    return tmpVal;
                }
            }
            throw new BaseException("Failed to convert");
        }
        public static TResult ConvertEnum<TSource, TResult>(this TSource source)
            where TSource : struct
            where TResult : struct
        {
            if (!typeof(TSource).IsEnum || !typeof(TResult).IsEnum)
                throw new InvalidOperationException("Operation must be carried out between enums only");
            return (TResult)(object)(int)(object)source;
        }
        public static Nullable<TResult> ConvertNullableEnum<TSource, TResult>(this Nullable<TSource> source)
            where TSource : struct
            where TResult : struct
        {
            if (!typeof(TSource).IsEnum || !typeof(TResult).IsEnum)
                throw new InvalidOperationException("Operation must be carried out between enums only");
            if (!source.HasValue)
                return null;
            return source.Value.ConvertEnum<TSource, TResult>();
        }

        public static string StripHtml(this string html)
        {
            if (html == null)
                return null;
            return Regex.Replace(html, @"<[^>]*>", string.Empty);
        }
        public static string Truncate(this string value, int chars)
        {
            if (value == null)
                return null;
            if (value.Length <= chars)
                return value;
            return string.Format("{0}...", value.Substring(0, chars));
        }
        public static string Spaces(this int count)
        {
            return "".PadLeft(count, ' ');
        }
        public static bool IsWhitespace(this char @char)
        {
            return char.IsWhiteSpace(@char);
        }
        public static T PropertyValueSafe<T>(this object obj, string name)
        {
            Guard.PropertyNotNullOrEmpty(name, "name");
            Guard.ParamNotNull(obj, "obj");
            var info = obj
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (info == null)
                return default;
            return (T)info.GetValue(obj, null);
        }
        public static T PropertyValue<T>(this object obj, string name)
        {
            Guard.PropertyNotNullOrEmpty(name, "name");
            Guard.ParamNotNull(obj, "obj");
            var info = obj
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (info == null)
                throw new NotFoundException("No such property");
            return (T)info.GetValue(obj, null);
        }
        public static void SafeDispose(this IDisposable disposable)
        {
            if (disposable != null)
                disposable.Dispose();
        }

        public static byte[] ToUtf8Bytes(this string value)
        {
            if (value == null)
                throw new NullReferenceException();
            return Encoding.UTF8.GetBytes(value);
        }

        public static string NormalizePhone(this string phone)
        {
            Guard.PropertyNotNullOrEmpty(phone, nameof(phone));
            var sb = new StringBuilder(phone);
            for (var i = sb.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(sb[i]))
                {
                    sb.Remove(i, 1);
                }
            }
            return sb.ToString();
        }

        public static string HtmlEncode(this string value)
        {
            if (value == null)
                throw new NullReferenceException();
            var chars = new Dictionary<char, string> {
                {'"', "&quot;"},
                {'&', "&amp;"},
                {'<', "&lt;"},
                {'>', "&gt;"},
                {'\'', "&#39;"}
            };
            var sb = new StringBuilder();
            for (var i = 0; i < value.Length; i++)
            {
                if (chars.ContainsKey(value[i]))
                    sb.Append(chars[value[i]]);
                else
                    sb.Append(value[i]);
            }
            return sb.ToString();
        }
        public static string UrlEncode(this string value)
        {
            if (value == null)
                throw new NullReferenceException();
            return System.Uri.EscapeDataString(value);
        }
        /// <summary>
        /// Extension method to test whether the value is a base64 string
        /// </summary>
        /// <param name="value">Value to test</param>
        /// <returns>bool value, true if the string is base64, otherwise false</returns>
        public static bool IsBase64String(this string value)
        {
            if (value == null || value.Length == 0 || value.Length % 4 != 0
                || value.Contains(' ') || value.Contains('\t') || value.Contains('\r') || value.Contains('\n'))
                return false;
            var index = value.Length - 1;
            if (value[index] == '=')
                index--;
            if (value[index] == '=')
                index--;
            for (var i = 0; i <= index; i++)
                if (IsInvalidBase64Char(value[i]))
                    return false;
            return true;
        }
        public static string ToCsvLine(this string[] data)
        {
            return data.Aggregate(new StringBuilder(), (sb, c) =>
                sb.AppendFormat("{0},", c.Return(x => x.Contains(',') ? "\"" + c.Replace("\"", "\"\"") + "\"" : c, ""))
            ).RemoveLast().ToString();
        }
        public static StringBuilder RemoveLast(this StringBuilder sb, int count = 1)
        {
            if (sb == null)
                throw new NullReferenceException();
            if (count < 0 || count > sb.Length)
                throw new ArgumentOutOfRangeException();
            return sb.Remove(sb.Length - count, count);
        }

        /// <summary>
        /// Formatting the string 
        /// </summary>
        /// <param name="value">value to format</param>
        /// <param name="args">replacement arguments</param>
        /// <returns></returns>
        public static string BraceFormat(this string value, Dictionary<string, object> args)
        {
            if (value == null)
                throw new NullReferenceException();
            var format = new BraceFormat(value, args);
            return format.ToString();
        }
        public static string BraceFormat(this string value, object vals)
        {
            if (vals == null)
                throw new ArgumentNullException("vals");
            if (value == null)
                throw new ArgumentNullException("value");
            var props = vals.GetType().GetProperties();
            var dic = new Dictionary<string, object>(props.Length);
            foreach (var prop in props)
                dic[prop.Name] = prop.GetValue(vals, null);
            return value.BraceFormat(dic);
        }

        private static bool IsInvalidBase64Char(char value)
        {
            var intValue = (int)value;
            if (intValue >= 48 && intValue <= 57)
                return false;
            if (intValue >= 65 && intValue <= 90)
                return false;
            if (intValue >= 97 && intValue <= 122)
                return false;
            return intValue != 43 && intValue != 47;
        }
    }
}