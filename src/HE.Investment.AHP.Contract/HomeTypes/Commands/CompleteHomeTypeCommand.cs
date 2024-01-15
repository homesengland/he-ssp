using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record CompleteHomeTypeCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, IsSectionCompleted IsSectionCompleted)
    : ISaveHomeTypeSegmentCommand;
