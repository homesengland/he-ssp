namespace HE.Investments.Common.Utils.Pagination;

public record PaginationRequest(int Page, int ItemsPerPage = DefaultPagination.PageSize);
