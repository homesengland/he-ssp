using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveModernMethodsConstructionCategoriesCommand(
        string ApplicationId,
        string HomeTypeId,
        IReadOnlyCollection<ModernMethodsConstructionCategoriesType> ModernMethodsConstructionCategories)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
