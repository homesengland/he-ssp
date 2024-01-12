using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Contract.UserOrganisation;
using MediatR;

namespace HE.Investments.Account.Contract.UserOrganisation.Queries;

public record GetUserOrganisationWithProgrammesQuery : IRequest<GetUserOrganisationWithProgrammesQueryResponse>;

public record GetUserOrganisationWithProgrammesQueryResponse(
    OrganizationBasicInformation OrganizationBasicInformation,
    string? UserFirstName,
    bool IsLimitedUser,
    IList<Programme> ProgrammesToAccess,
    IList<ProgrammeType> ProgrammesTypesToApply);
