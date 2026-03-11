using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace JWueller.Jellyfin.OnePace;

internal static class IdentifierUtil
{
    public static readonly Regex OnePaceInvariantTitleRegex = BuildTextRegex("One Pace");

    [SuppressMessage("ReSharper", "StringLiteralTypo", Justification = "Regex")]
    public static Regex BuildTextRegex(string needle)
    {
        var pattern = @"\b" + string.Join(@"\s+", needle.Split().Select(Regex.Escape)) + @"\b";

        pattern = pattern.Replace("Whisky", "Whiske?y", StringComparison.InvariantCultureIgnoreCase);

        return new Regex(pattern, RegexOptions.IgnoreCase);
    }

    public static Regex BuildChapterRangeRegex(string chapters)
    {
        var normalized = chapters.Trim();

        var escaped = Regex.Escape(normalized);
        var withOptionalBrackets = escaped.Replace(@"\(", @"\[?", StringComparison.Ordinal).Replace(@"\)", @"\]?", StringComparison.Ordinal);
        var withOptionalCommas = withOptionalBrackets.Replace(",", @"[\s,]*", StringComparison.Ordinal);
        var withOptionalCoverStories = withOptionalCommas.Replace("cover stories", @"(\s+cover\s+stories)?", StringComparison.OrdinalIgnoreCase);

        var pattern = $@"\b{withOptionalCoverStories}\b";
        return new Regex(pattern, RegexOptions.IgnoreCase);
    }
}
