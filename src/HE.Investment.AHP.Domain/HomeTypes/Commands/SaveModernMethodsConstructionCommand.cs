using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveModernMethodsConstructionCommand(string ApplicationId, string HomeTypeId, YesNoType ModernMethodsConstructionApplied)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
