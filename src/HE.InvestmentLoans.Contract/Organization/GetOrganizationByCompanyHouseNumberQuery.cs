using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Organization;

public record GetOrganizationByCompanyHouseNumberQuery(string CompanyHouseNumber) : IRequest<OrganizationBasicDetails>;

