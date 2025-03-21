namespace CulturalShare.Common.Helper.Extensions;

public static class CollectionHelper
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> arr)
    {
        return arr == null || !arr.Any();
    }

    public static IEnumerable<IEnumerable<T>> SplitToParts<T>(this IEnumerable<T> list, int partitionSize = 200)
    {
        return list
            .Select((item, inx) => new { item, inx })
            .GroupBy(x => x.inx / partitionSize)
            .Select(g => g.Select(x => x.item));
    }
}
