using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveExemptFromTheRightToSharedOwnershipCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, YesNoType ExemptFromTheRightToSharedOwnership)
    : ISaveHomeTypeSegmentCommand;
