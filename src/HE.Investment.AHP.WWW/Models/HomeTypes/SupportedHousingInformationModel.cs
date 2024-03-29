using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class SupportedHousingInformationModel : HomeTypeBasicModel
{
    public SupportedHousingInformationModel(string applicationName, string homeTypeName)
        : base(applicationName, homeTypeName)
    {
    }

    public SupportedHousingInformationModel()
        : this(string.Empty, string.Empty)
    {
    }

    public YesNoType LocalCommissioningBodiesConsulted { get; set; }

    public YesNoType ShortStayAccommodation { get; set; }

    public RevenueFundingType RevenueFundingType { get; set; }
}
