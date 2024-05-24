namespace HE.Investments.TestsUtils.Extensions;

public static class ListExtensions
{
    public static T PickNthItem<T>(this IList<T> items, int itemIndex)
    {
        if (itemIndex >= items.Count)
        {
            itemIndex -= itemIndex / items.Count * items.Count;
        }

        return items[itemIndex];
    }
}
