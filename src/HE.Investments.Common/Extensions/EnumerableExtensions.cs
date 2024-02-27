using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Utils;

namespace HE.Investments.Common.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> TakePage<T>(this IEnumerable<T> source, PaginationRequest paging)
    {
        return source
            .Skip(paging.ItemsPerPage * (paging.Page - 1))
            .Take(paging.ItemsPerPage);
    }

    public static DisposableList<T> ToDisposableList<T>(this IEnumerable<T> source)
        where T : IDisposable
    {
        return new DisposableList<T>(source);
    }
}
