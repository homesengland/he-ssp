using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Shared;

public record OrganisationBasicInfo(
    OrganisationId OrganisationId,
    string RegisteredCompanyName,
    string CompanyRegistrationNumber,
    string AddressLine1,
    bool IsUnregisteredBody);
