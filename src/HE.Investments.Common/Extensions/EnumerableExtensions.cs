using HE.Investments.Common.Utils.Pagination;

namespace HE.Investments.Common.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> TakePage<T>(this IEnumerable<T> source, PaginationRequest paging)
    {
        return source
            .Skip(paging.ItemsPerPage * (paging.Page - 1))
            .Take(paging.ItemsPerPage);
    }
}
