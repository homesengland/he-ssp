using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.LocalAuthority.Queries;

public record GetLocalAuthoritiesQuery(PaginationRequest PaginationRequest, string Phrase) : IRequest<PaginationResult<Common.Contract.LocalAuthority>>;
