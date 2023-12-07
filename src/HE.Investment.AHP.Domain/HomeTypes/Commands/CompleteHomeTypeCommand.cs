using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record CompleteHomeTypeCommand(string ApplicationId, string HomeTypeId, IsSectionCompleted IsSectionCompleted)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
