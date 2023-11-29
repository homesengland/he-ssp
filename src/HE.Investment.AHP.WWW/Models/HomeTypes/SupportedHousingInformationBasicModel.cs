using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class SupportedHousingInformationBasicModel : HomeTypeBasicModel
{
    public SupportedHousingInformationBasicModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public SupportedHousingInformationBasicModel()
        : this(string.Empty, string.Empty)
    {
    }

    public YesNoType LocalCommissioningBodiesConsulted { get; set; }

    public YesNoType ShortStayAccommodation { get; set; }

    public RevenueFundingType RevenueFundingType { get; set; }
}
