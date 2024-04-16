using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;

public class RequiredFundingMapper : EnumMapper<RequiredFundingOption>
{
    protected override IDictionary<RequiredFundingOption, int?> Mapping => new Dictionary<RequiredFundingOption, int?>
    {
        { RequiredFundingOption.LessThan250K, (int)he_Pipeline_he_fundingask.Lessthan250000 },
        { RequiredFundingOption.Between250KAnd1Mln, (int)he_Pipeline_he_fundingask._250000to1million },
        { RequiredFundingOption.Between1MlnAnd5Mln, (int)he_Pipeline_he_fundingask._1millionto5million },
        { RequiredFundingOption.Between5MlnAnd10Mln, (int)he_Pipeline_he_fundingask._5millionto10million },
        { RequiredFundingOption.Between10MlnAnd30Mln, (int)he_Pipeline_he_fundingask._10millionto30million },
        { RequiredFundingOption.Between30MlnAnd50Mln, (int)he_Pipeline_he_fundingask._30millionto50million },
        { RequiredFundingOption.MoreThan50Mln, (int)he_Pipeline_he_fundingask.Morethan50million },
        { RequiredFundingOption.IDoNotKnow, (int)he_Pipeline_he_fundingask.Idontknow },
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
