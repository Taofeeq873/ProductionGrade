using System.Text;
using System.Text.RegularExpressions;

namespace Application.Extensions;

public static partial class StringExtensions
{
    public static string ToCamelCase(this string str)
    {
        var chars = MyRegex().Replace(str, " ").ToCharArray();
        var sb = new StringBuilder();
        for (var i = 0; i < chars.Length; i++)
            if (i == 0)
                sb.Append(char.ToLowerInvariant(chars[i]));
            else if (char.IsWhiteSpace(chars[i - 1]))
                sb.Append(char.ToUpperInvariant(chars[i]));
            else if (char.IsUpper(chars[i]) && char.IsUpper(chars[i - 1]) && (i + 1 >= chars.Length ||
                                                                              char.IsUpper(chars[i + 1]) ||
                                                                              char.IsWhiteSpace(chars[i + 1])))
                sb.Append(char.ToLowerInvariant(chars[i]));
            else
                sb.Append(chars[i]);
        return sb.ToString().Replace(" ", "");
    }

    [GeneratedRegex("[^A-Za-z0-9]")]
    private static partial Regex MyRegex();
}