using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class RequiredFundingMapper : EnumMapper<RequiredFundingOption>
{
    protected override IDictionary<RequiredFundingOption, int?> Mapping => FrontDoorProjectEnumMapping.FundingAmount;

    public int? Map(RequiredFunding value)
    {
        return value.Value is null ? null : ToDto(value.Value)!.Value;
    }

    public RequiredFunding Map(int? value)
    {
        return value is null ? RequiredFunding.Empty : new RequiredFunding(ToDomain(value)!.Value);
    }
}
