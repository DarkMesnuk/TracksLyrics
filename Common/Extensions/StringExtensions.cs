namespace Common.Extensions;

public static class StringExtensions
{
    public static string ToNormalized(this string value)
    {
        return value.Trim()
            .ToUpperInvariant();
    }
    
    public static string ToNormalizedWithoutTrim(this string value)
    {
        return value
            .ToUpperInvariant();
    }
}