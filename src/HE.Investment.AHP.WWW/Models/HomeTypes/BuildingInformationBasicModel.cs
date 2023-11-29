using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class BuildingInformationBasicModel : HomeTypeBasicModel
{
    public BuildingInformationBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public BuildingInformationBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public BuildingType BuildingType { get; set; }
}
