using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class TypeOfFacilitiesModel : ProvidedHomeTypeModelBase
{
    public TypeOfFacilitiesModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public TypeOfFacilitiesModel()
        : this(string.Empty, string.Empty)
    {
    }

    public FacilityType FacilityType { get; set; }
}
