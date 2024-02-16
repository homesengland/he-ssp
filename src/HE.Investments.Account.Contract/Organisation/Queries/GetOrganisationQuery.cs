using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Queries;

public record GetOrganisationQuery(string CompanyHouseNumberOrOrganisationId) : IRequest<OrganisationBasicDetails>;
