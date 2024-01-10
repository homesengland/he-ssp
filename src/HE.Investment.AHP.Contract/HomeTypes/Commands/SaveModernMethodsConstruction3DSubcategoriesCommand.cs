using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveModernMethodsConstruction3DSubcategoriesCommand(
        AhpApplicationId ApplicationId,
        string HomeTypeId,
        IReadOnlyCollection<ModernMethodsConstruction3DSubcategoriesType> ModernMethodsConstruction3DSubcategories)
    : ISaveHomeTypeSegmentCommand;
