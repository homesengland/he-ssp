namespace HE.InvestmentLoans.Common.Extensions;

public static class CollectionExtensions
{
    public static void AddRange<T>(this IList<T> source, IList<T> itemsToAdd)
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
}
