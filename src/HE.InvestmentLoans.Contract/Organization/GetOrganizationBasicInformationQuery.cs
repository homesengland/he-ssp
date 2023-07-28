using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Organization;

public record GetOrganizationBasicInformationQuery() : IRequest<GetOrganizationBasicInformationQueryResponse>;

public record GetOrganizationBasicInformationQueryResponse(OrganizationBasicInformation OrganizationBasicInformation);
