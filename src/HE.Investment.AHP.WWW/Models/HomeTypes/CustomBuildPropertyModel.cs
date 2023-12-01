using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class CustomBuildPropertyModel : HomeTypeBasicModel
{
    public CustomBuildPropertyModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public CustomBuildPropertyModel()
        : this(string.Empty, string.Empty)
    {
    }

    public YesNoType CustomBuild { get; set; }
}
