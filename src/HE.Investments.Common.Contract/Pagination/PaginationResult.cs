namespace HE.Investments.Common.Contract.Pagination;

public record PaginationResult<TItem>(IList<TItem> Items, int CurrentPage, int ItemsPerPage, int TotalItems)
{
    public bool Any() => Items.Any();
}
