using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveRevenueFundingCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, IReadOnlyCollection<RevenueFundingSourceType> Sources)
    : ISaveHomeTypeSegmentCommand;
