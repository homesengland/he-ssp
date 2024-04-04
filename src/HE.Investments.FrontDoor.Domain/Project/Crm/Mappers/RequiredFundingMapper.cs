using HE.Investments.Common.CRM.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class RequiredFundingMapper : EnumMapper<RequiredFundingOption>
{
    private readonly IFrontDoorProjectEnumMapping _mapping;

    public RequiredFundingMapper(IFrontDoorProjectEnumMapping mapping)
    {
        _mapping = mapping;
    }

    protected override IDictionary<RequiredFundingOption, int?> Mapping => _mapping.FundingAmount;

    public int? Map(RequiredFunding value)
    {
        return value.Value is null ? null : ToDto(value.Value)!.Value;
    }

    public RequiredFunding Map(int? value)
    {
        return value is null ? RequiredFunding.Empty : new RequiredFunding(ToDomain(value)!.Value);
    }
}
