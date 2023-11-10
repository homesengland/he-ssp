using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Queries;

public record GetOrganizationQuery(string CompanyHouseNumberOrOrganisationId) : IRequest<OrganizationBasicDetails>;
