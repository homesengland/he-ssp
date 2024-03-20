using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Shared.Project.Data;

public record ProjectPrefillData(
    FrontDoorProjectId Id,
    bool IsEnglandHousingDelivery,
    string Name,
    IReadOnlyCollection<SupportActivityType> SupportActivities,
    AffordableHomesAmount? AffordableHomesAmount,
    int? OrganisationHomesBuilt,
    IReadOnlyCollection<InfrastructureType> InfrastructureTypes,
    bool IsSiteIdentified,
    SiteDetails? Site,
    SiteNotIdentifiedDetails? SiteNotIdentified,
    bool IsSupportRequired,
    bool IsFundingRequired,
    FundingDetails? FundingDetails,
    DateDetails ExpectedStartDate);
