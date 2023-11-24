using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveRevenueFundingCommand(string ApplicationId, string HomeTypeId, IReadOnlyCollection<RevenueFundingSourceType> Sources)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
