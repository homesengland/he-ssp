using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveExemptFromTheRightToSharedOwnershipCommand(string ApplicationId, string HomeTypeId, YesNoType ExemptFromTheRightToSharedOwnership)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
