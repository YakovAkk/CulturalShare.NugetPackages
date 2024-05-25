namespace CulturalShare.Common.Helper.Extensions;

public static class EnumExtensions
{
    public static T ToEnum<T>(this string value) where T : struct, Enum
    {
        if (Enum.TryParse(value, true, out T result))
        {
            return result;
        }
        throw new ArgumentException($"Unable to convert '{value}' to enum {typeof(T).Name}");
    }
}
