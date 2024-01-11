using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveModernMethodsConstruction2DSubcategoriesCommand(
        AhpApplicationId ApplicationId,
        string HomeTypeId,
        IReadOnlyCollection<ModernMethodsConstruction2DSubcategoriesType> ModernMethodsConstruction2DSubcategories)
    : ISaveHomeTypeSegmentCommand;
