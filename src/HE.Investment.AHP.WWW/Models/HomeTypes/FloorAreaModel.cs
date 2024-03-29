using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class FloorAreaModel : HomeTypeBasicModel
{
    public FloorAreaModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public FloorAreaModel()
        : this(string.Empty, string.Empty)
    {
    }

    public string? FloorArea { get; set; }

    public YesNoType MeetNationallyDescribedSpaceStandards { get; set; }

    public IList<NationallyDescribedSpaceStandardType>? NationallyDescribedSpaceStandards { get; set; }

    public IList<NationallyDescribedSpaceStandardType>? OtherNationallyDescribedSpaceStandards { get; set; }
}
