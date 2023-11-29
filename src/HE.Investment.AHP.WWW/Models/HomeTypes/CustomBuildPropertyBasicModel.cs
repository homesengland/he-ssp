using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class CustomBuildPropertyBasicModel : HomeTypeBasicModel
{
    public CustomBuildPropertyBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public CustomBuildPropertyBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public YesNoType CustomBuild { get; set; }
}
