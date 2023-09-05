using MediatR;

namespace HE.InvestmentLoans.Contract.Organization;
public record SearchOrganizationsInCrm(string SearchPhrase) : IRequest<SearchOrganizationsInCrmResponse>;

public record SearchOrganizationsInCrmResponse(OrganizationViewModel Organization);
