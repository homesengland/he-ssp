using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class MoveOnAccommodationBasicModel : HomeTypeBasicModel
{
    public MoveOnAccommodationBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public MoveOnAccommodationBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public YesNoType IntendedAsMoveOnAccommodation { get; set; }
}
