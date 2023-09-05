using MediatR;

namespace HE.InvestmentLoans.Contract.Organization;
public record SearchOrganizations(string SearchPhrase) : IRequest<SearchOrganisationResponse>;

public record SearchOrganisationResponse(OrganizationViewModel Result);
