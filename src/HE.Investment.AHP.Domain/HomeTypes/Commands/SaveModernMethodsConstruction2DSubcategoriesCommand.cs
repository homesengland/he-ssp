using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveModernMethodsConstruction2DSubcategoriesCommand(
        string ApplicationId,
        string HomeTypeId,
        IReadOnlyCollection<ModernMethodsConstruction2DSubcategoriesType> ModernMethodsConstruction2DSubcategories)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
