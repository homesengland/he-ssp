using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveAccessibilityCategoryCommand(string ApplicationId, string HomeTypeId, AccessibilityCategoryType AccessibilityCategory)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
