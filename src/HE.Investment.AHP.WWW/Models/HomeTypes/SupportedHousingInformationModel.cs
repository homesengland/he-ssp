using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public class SupportedHousingInformationModel : ProvidedHomeTypeModelBase
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
