using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Contract.Application.Queries;

public record GetApplicationsQueryResult(string OrganisationName, PaginationResult<ApplicationBasicDetails> PaginationResult);
