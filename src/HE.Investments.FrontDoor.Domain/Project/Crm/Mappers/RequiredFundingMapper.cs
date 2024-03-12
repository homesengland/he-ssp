using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Contract.Project.Enums;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class RequiredFundingMapper : EnumMapper<RequiredFundingOption>
{
    protected override IDictionary<RequiredFundingOption, int?> Mapping => new Dictionary<RequiredFundingOption, int?>
    {
        { RequiredFundingOption.LessThan250K, (int)invln_FrontDoorAmountofFundingRequired.Lessthan250000 },
        { RequiredFundingOption.Between250KAnd1Mln, (int)invln_FrontDoorAmountofFundingRequired._250000to1million },
        { RequiredFundingOption.Between1MlnAnd5Mln, (int)invln_FrontDoorAmountofFundingRequired._1millionto5million },
        { RequiredFundingOption.Between5MlnAnd10Mln, (int)invln_FrontDoorAmountofFundingRequired._5millionto10million },
        { RequiredFundingOption.Between10MlnAnd30Mln, (int)invln_FrontDoorAmountofFundingRequired._10millionto30million },
        { RequiredFundingOption.Between30MlnAnd50Mln, (int)invln_FrontDoorAmountofFundingRequired._30millionto50million },
        { RequiredFundingOption.MoreThan50Mln, (int)invln_FrontDoorAmountofFundingRequired.Morethan50million },
        { RequiredFundingOption.IDoNotKnow, (int)invln_FrontDoorAmountofFundingRequired.Idonotknow },
    };

    public int? Map(RequiredFunding value)
    {
        return value.Value is null ? null : ToDto(value.Value)!.Value;
    }

    public RequiredFunding Map(int? value)
    {
        return value is null ? RequiredFunding.Empty : new RequiredFunding(ToDomain(value)!.Value);
    }
}
