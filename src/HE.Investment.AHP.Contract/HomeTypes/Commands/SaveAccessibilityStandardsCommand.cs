using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveAccessibilityStandardsCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, YesNoType AccessibilityStandards)
    : ISaveHomeTypeSegmentCommand;
