using HE.Investments.Account.Shared.User.ValueObjects;

namespace HE.Investments.Account.Shared;

public record OrganisationBasicInfo(
    OrganisationId OrganisationId,
    string RegisteredCompanyName,
    string CompanyRegistrationNumber,
    string AddressLine1,
    bool IsUnregisteredBody);
