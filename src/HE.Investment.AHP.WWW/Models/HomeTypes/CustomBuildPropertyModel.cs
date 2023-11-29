using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class CustomBuildPropertyModel : ProvidedHomeTypeModelBase
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
