using System.Text;
namespace EBook.Core.Helpers
{
    public static class StringBuilderExtensions
    {
        public static int LastIndexOf(this StringBuilder sb, Char c)
        {
            if (sb == null)
                throw new NullReferenceException();
            for (var i = sb.Length - 1; i >= 0; i--)
                if (sb[i] == c)
                    return i;
            return -1;
        }
        public static void Shuffle(this StringBuilder sb)
        {
            var list = new List<int>(sb.Length);
            var rnd = new Random();
            for (var i = 0; i < sb.Length; i++)
            {
                var index = rnd.Next(0, list.Count + 1);
                list.Insert(index, i);
            }
            var current = sb.ToString();
            for (var i = 0; i < sb.Length; i++)
                sb[i] = current[list[i]];
        }
        public static StringBuilder InsertFormat(this StringBuilder sb, int index, String format, params Object[] args)
        {
            if (sb == null)
                throw new NullReferenceException();
            var result = String.Format(format, args);
            return sb.Insert(index, result);
        }
        public static StringBuilder AppendStringBuilder(this StringBuilder sb, StringBuilder second)
        {
            for (var i = 0; i < second.Length; i++)
                sb.Append(second[i]);
            return sb;
        }
    }
}