using GorillaLibrary.Utilities;
using System.Collections.Generic;
using System.Globalization;

namespace GorillaLibrary.Extensions;

public static class StringExtensions
{
    private static readonly CultureInfo cultureInfo = CultureInfo.InvariantCulture;

    private static readonly TextInfo _textInfo = cultureInfo.TextInfo;

    private static readonly Dictionary<string, string> _nameCache = [];

    /// <summary>
    /// Sanitizes a string to how it would be shown on scoreboards and player names.
    /// </summary>
    /// <param name="name">String to sanitize</param>
    /// <returns>The sanitized string.</returns>
    public static string SanitizeName(this string name)
    {
        name ??= string.Empty;

        if (_nameCache.TryGetValue(name, out string sanitizedName))
            return sanitizedName;

        sanitizedName = RigUtility.LocalRig.NormalizeName(true, name);
        _nameCache.TryAdd(name, sanitizedName);

        return sanitizedName;
    }

    /// <summary>
    /// Converts a string to title case.
    /// </summary>
    /// <param name="original">The original string to convert.</param>
    /// <param name="forceLower">Whether to convert the string to lowercase before applying title case.</param>
    /// <returns>The string converted to title case.</returns>
    public static string ToTitleCase(this string original, bool forceLower = true) => _textInfo.ToTitleCase(forceLower ? original.ToLower() : original);

    /// <summary>
    /// Sanitizes a strings length to be forced to be a specified length.
    /// </summary>
    /// <param name="str">String to sanitize.</param>
    /// <param name="maxLength">Max length of the string specified.</param>
    /// <returns>The string with the specified max size.</returns>
    public static string LimitLength(this string str, int maxLength) => str.Length > maxLength ? str[..maxLength] : str;
}
