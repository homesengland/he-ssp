using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveAccessibilityStandardsCommand(string ApplicationId, string HomeTypeId, YesNoType AccessibilityStandards)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
