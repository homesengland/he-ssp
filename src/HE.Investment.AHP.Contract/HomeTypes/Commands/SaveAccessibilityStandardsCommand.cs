using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveAccessibilityStandardsCommand(AhpApplicationId ApplicationId, string HomeTypeId, YesNoType AccessibilityStandards)
    : ISaveHomeTypeSegmentCommand;
