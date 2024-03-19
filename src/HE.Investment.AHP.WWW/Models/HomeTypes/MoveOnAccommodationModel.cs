using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class MoveOnAccommodationModel : HomeTypeBasicModel
{
    public MoveOnAccommodationModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public MoveOnAccommodationModel()
        : this(string.Empty, string.Empty)
    {
    }

    public YesNoType IntendedAsMoveOnAccommodation { get; set; }
}
