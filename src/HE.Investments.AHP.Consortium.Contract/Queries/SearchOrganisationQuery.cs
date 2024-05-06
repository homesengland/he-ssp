using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investments.AHP.Consortium.Contract.Queries;

public record SearchOrganisationQuery(string Phrase, PaginationRequest PaginationRequest) : IRequest<SearchOrganisationResult>;
