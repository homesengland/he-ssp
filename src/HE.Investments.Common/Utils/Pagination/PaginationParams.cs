namespace HE.Investments.Common.Utils.Pagination;

public record PaginationParams
{
    public int TotalItems { get; set; }

    public int Page { get; set; }

    public int ItemsPerPage { get; set; }
}
