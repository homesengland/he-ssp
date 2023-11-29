using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class TypeBasicOfFacilitiesModel : HomeTypeBasicModel
{
    public TypeBasicOfFacilitiesModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public TypeBasicOfFacilitiesModel()
        : this(string.Empty, string.Empty)
    {
    }

    public FacilityType FacilityType { get; set; }
}
