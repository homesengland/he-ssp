using HE.Investments.Account.Contract.UserOrganisation;
using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Queries;

public class GetUserOrganisationInformationQuery : IRequest<GetUserOrganisationInformationQueryResponse>;

public record GetUserOrganisationInformationQueryResponse(
    OrganizationBasicInformation OrganizationBasicInformation,
    string? UserFirstName,
    bool IsLimitedUser,
    IList<Programme> ProgrammesToAccess,
    IList<ProgrammeType> ProgrammesTypesToApply);
