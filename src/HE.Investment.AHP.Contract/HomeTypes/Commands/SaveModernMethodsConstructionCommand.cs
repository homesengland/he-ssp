using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveModernMethodsConstructionCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, YesNoType ModernMethodsConstructionApplied)
    : ISaveHomeTypeSegmentCommand;
