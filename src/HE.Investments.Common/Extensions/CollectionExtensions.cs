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

    public static bool IsNotEmpty<T>(this IList<T> source)
    {
        return source.Count != 0;
    }

    public static bool IsEmpty<T>(this IList<T> source)
    {
        return source.Count == 0;
    }

    public static bool IsTheSameAs<T>(this IList<T> source, IList<T> other)
    {
        if (source.Count != other.Count)
        {
            return false;
        }

        for (var i = 0; i < source.Count; i++)
        {
            if (!source[i]!.Equals(other[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsNotTheSameAs<T>(this IList<T> source, IList<T> other) => !source.IsTheSameAs(other);

    public static TItem? PopItem<TItem>(this List<TItem> items)
        where TItem : class
    {
        if (items.Count > 0)
        {
            var result = items[0];
            items.RemoveAt(0);
            return result;
        }

        return null;
    }
}
