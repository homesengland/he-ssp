namespace HE.Investments.Common.Contract.Pagination;

public record PaginationRequest(int Page, int ItemsPerPage = DefaultPagination.PageSize);
