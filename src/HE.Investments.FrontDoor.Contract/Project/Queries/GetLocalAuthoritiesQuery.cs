using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Project.Queries;

public record GetLocalAuthoritiesQuery(PaginationRequest PaginationRequest, string Phrase) : IRequest<PaginationResult<LocalAuthority>>;
