using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investments.Organisation.Contract.Queries;

public record SearchOrganisationQuery(string Phrase, PaginationRequest PaginationRequest) : IRequest<SearchOrganisationResult>;
