using HE.Investment.AHP.Contract.Application;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.Contract;
using UploadedFile = HE.Investment.AHP.Contract.Common.UploadedFile;

namespace HE.Investment.AHP.Contract.Scheme;

public record Scheme(
    ApplicationDetails Application,
    SectionStatus Status,
    decimal? RequiredFunding,
    int? HousesToDeliver,
    OrganisationDetails? DevelopingPartner,
    OrganisationDetails? OwnerOfTheLand,
    OrganisationDetails? OwnerOfTheHomes,
    bool? ArePartnersConfirmed,
    string? AffordabilityEvidence,
    string? SalesRisk,
    string? MeetingLocalPriorities,
    string? MeetingLocalHousingNeed,
    string? StakeholderDiscussionsReport,
    UploadedFile? LocalAuthoritySupportFile,
    bool IsConsortiumMember);
