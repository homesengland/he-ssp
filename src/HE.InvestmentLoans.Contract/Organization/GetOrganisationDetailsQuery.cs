using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Organization;

public record GetOrganisationDetailsQuery() : IRequest<GetOrganisationDetailsQueryResponse>;

public record GetOrganisationDetailsQueryResponse(OrganisationDetailsViewModel OrganisationDetailsViewModel);
