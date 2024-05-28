using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.HomeTypes;

public class GeneralHomeTypeData : HomeTypeDataBase<GeneralHomeTypeData>
{
    public GeneralHomeTypeData()
    {
    }

    public override HousingType HousingType => HousingType.General;

    public YesNoType MoveOnAccommodation { get; private set; }

    protected override GeneralHomeTypeData HomeType => this;

    public void SetHomeTypeId(string homeTypeId)
    {
        Id = homeTypeId;
    }

    public override GeneralHomeTypeData GenerateHomeTypeDetails()
    {
        Name = $"IT-General-{GenerateDateString()}";
        return this;
    }

    public GeneralHomeTypeData GenerateMoveOnAccommodation()
    {
        MoveOnAccommodation = YesNoType.Yes;
        return this;
    }
}
