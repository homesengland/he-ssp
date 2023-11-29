using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HappiDesignPrinciplesBasicModel : HomeTypeBasicModel
{
    public HappiDesignPrinciplesBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public HappiDesignPrinciplesBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public IList<HappiDesignPrincipleType>? DesignPrinciples { get; set; }

    public IList<HappiDesignPrincipleType>? OtherPrinciples { get; set; }
}
