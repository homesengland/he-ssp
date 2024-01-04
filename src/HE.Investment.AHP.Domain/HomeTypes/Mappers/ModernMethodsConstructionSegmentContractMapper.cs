using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public class ModernMethodsConstructionSegmentContractMapper : IHomeTypeSegmentContractMapper<ModernMethodsConstructionEntity, ModernMethodsConstruction>
{
    public ModernMethodsConstruction Map(ApplicationName applicationName, HomeTypeName homeTypeName, ModernMethodsConstructionEntity segment)
    {
        return new ModernMethodsConstruction(
            applicationName.Name,
            homeTypeName.Value,
            segment.ModernMethodsConstructionApplied,
            segment.ModernMethodsConstructionCategories.ToList(),
            segment.ModernMethodsConstruction2DSubcategories.ToList(),
            segment.ModernMethodsConstruction3DSubcategories.ToList());
    }
}
