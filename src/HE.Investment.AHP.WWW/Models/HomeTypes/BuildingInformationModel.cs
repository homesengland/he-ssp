using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class BuildingInformationModel : ProvidedHomeTypeModelBase
{
    public BuildingInformationModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public BuildingInformationModel()
        : this(string.Empty, string.Empty)
    {
    }

    public BuildingType BuildingType { get; set; }
}
