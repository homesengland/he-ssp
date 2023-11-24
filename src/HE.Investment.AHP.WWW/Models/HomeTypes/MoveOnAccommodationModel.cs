using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class MoveOnAccommodationModel : ProvidedHomeTypeModelBase
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
