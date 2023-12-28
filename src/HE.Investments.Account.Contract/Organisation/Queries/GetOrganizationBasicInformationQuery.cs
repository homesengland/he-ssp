using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Queries;

public record GetOrganizationBasicInformationQuery : IRequest<GetOrganizationBasicInformationQueryResponse>;

public record GetOrganizationBasicInformationQueryResponse(OrganizationBasicInformation OrganizationBasicInformation);
