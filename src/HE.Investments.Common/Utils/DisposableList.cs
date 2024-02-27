namespace HE.Investments.Common.Utils;

public sealed class DisposableList<T> : List<T>, IDisposable
    where T : IDisposable
{
    public DisposableList(IEnumerable<T> collection)
        : base(collection)
    {
    }

    public void Dispose()
    {
        foreach (var item in this)
        {
            item.Dispose();
        }
    }
}
