using HE.Investments.Account.Api.Contract.Organisation;

namespace HE.Investments.Account.Api.Contract.User;

public record UserOrganisation(OrganisationDetails Organisation, IReadOnlyCollection<UserRole> Roles);
