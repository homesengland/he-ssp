namespace HE.Investments.Common.Extensions;

public static class CollectionExtensions
{
    public static void AddRange<T>(this IList<T> source, IList<T>? itemsToAdd)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (itemsToAdd == null)
        {
            return;
        }

        foreach (var item in itemsToAdd)
        {
            source.Add(item);
        }
    }

    public static IList<T> Append<T>(this IList<T> source, IList<T>? itemsToAdd)
    {
        var copy = new List<T>(source);
        copy.AddRange(itemsToAdd);
        return copy;
    }

    public static void AddWhen<T>(this IList<T> source, T itemToAdd, bool shouldAdd)
    {
        if (shouldAdd)
        {
            source.Add(itemToAdd);
        }
    }
}
