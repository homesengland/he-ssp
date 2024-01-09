namespace HE.Investments.Common.Utils.Pagination;

public record PaginationResult<TItem>(IList<TItem> Items, int CurrentPage, int ItemsPerPage, int TotalItems)
{
    public bool Any() => Items.Any();
}
