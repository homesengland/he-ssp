using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveAccessibilityCategoryCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, AccessibilityCategoryType AccessibilityCategory)
    : ISaveHomeTypeSegmentCommand;
