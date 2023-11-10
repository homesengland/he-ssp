using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class HappiDesignPrinciplesModel : ProvidedHomeTypeModelBase
{
    public HappiDesignPrinciplesModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public HappiDesignPrinciplesModel()
        : this(string.Empty, string.Empty)
    {
    }

    public IList<HappiDesignPrincipleType> DesignPrinciples { get; set; }

    public IList<HappiDesignPrincipleType> OtherPrinciples { get; set; }
}
