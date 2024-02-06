using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Queries;

public class GetOrganisationBasicInformationQuery : IRequest<GetOrganizationBasicInformationQueryResponse>
{
}

public record GetOrganizationBasicInformationQueryResponse(OrganizationBasicInformation OrganisationBasicInformation);
