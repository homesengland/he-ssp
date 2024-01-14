using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Queries;

public class GetOrganizationBasicInformationQuery : IRequest<GetOrganizationBasicInformationQueryResponse>
{
}

public record GetOrganizationBasicInformationQueryResponse(OrganizationBasicInformation OrganizationBasicInformation);
