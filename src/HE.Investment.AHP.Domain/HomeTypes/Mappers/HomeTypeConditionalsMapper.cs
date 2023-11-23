using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public static class HomeTypeConditionalsMapper
{
    public static HomeTypeConditionals Map(IHomeTypeEntity homeType)
    {
        return new HomeTypeConditionals(
            homeType.SupportedHousingInformation.LocalCommissioningBodiesConsulted,
            homeType.SupportedHousingInformation.ShortStayAccommodation,
            homeType.SupportedHousingInformation.RevenueFundingType);
    }
}
