namespace HE.Investments.Common.Extensions;

public static class DictionaryExtensions
{
    public static bool TryGetKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TValue value, out TKey? key)
    {
        var status = source
            .Where(t => t.Value != null && t.Value!.Equals(value))
            .Select(t => t.Key)
            .FirstOrDefault();

        if (status == null)
        {
            key = default;
            return false;
        }
        else
        {
            key = (TKey?)status;
            return true;
        }
    }
}
