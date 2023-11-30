using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class AccessibilityModel : HomeTypeBasicModel
{
    public AccessibilityModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public AccessibilityModel()
        : this(string.Empty, string.Empty)
    {
    }

    public YesNoType AccessibilityStandards { get; set; }

    public AccessibilityCategoryType AccessibilityCategory { get; set; }
}
