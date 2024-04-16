using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Queries;

public record SearchLocalAuthoritiesQuery(string Phrase, PaginationRequest PaginationRequest) : IRequest<OperationResult<LocalAuthorities>>;
