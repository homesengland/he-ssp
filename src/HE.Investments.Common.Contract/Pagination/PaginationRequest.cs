namespace HE.Investments.Common.Contract.Pagination;

public record PaginationRequest(int Page, int ItemsPerPage = DefaultPagination.PageSize)
{
    public static PaginationRequest All => new(1, 1000);
}
