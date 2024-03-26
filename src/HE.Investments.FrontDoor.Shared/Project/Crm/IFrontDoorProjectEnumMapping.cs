using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Shared.Project.Crm;

public interface IFrontDoorProjectEnumMapping
{
    public IDictionary<SupportActivityType, int?> ActivityType { get; }

    public IDictionary<AffordableHomesAmount, int?> AffordableHomes { get; }

    public IDictionary<InfrastructureType, int?> Infrastructure { get; }

    public IDictionary<ProjectGeographicFocus, int?> GeographicFocus { get; }

    public IDictionary<RegionType, int?> RegionType { get; }

    public IDictionary<RequiredFundingOption, int?> FundingAmount { get; }
}
