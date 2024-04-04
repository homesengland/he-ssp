using HE.Investments.Account.Contract.Organisation.Queries;
using MediatR;

namespace HE.Investments.Account.Contract.UserOrganisation.Queries;

public class GetUserOrganisationWithProgrammesQuery : IRequest<GetUserOrganisationWithProgrammesQueryResponse>
{
}

public record GetUserOrganisationWithProgrammesQueryResponse(
    OrganizationBasicInformation OrganisationBasicInformation,
    string? UserFirstName,
    bool IsLimitedUser,
    IList<UserProject> Projects,
    IList<Programme> ProgrammesToAccess);
