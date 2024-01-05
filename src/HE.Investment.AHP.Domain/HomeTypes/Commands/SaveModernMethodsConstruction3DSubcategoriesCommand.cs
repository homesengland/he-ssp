using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveModernMethodsConstruction3DSubcategoriesCommand(
        string ApplicationId,
        string HomeTypeId,
        IReadOnlyCollection<ModernMethodsConstruction3DSubcategoriesType> ModernMethodsConstruction3DSubcategories)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
