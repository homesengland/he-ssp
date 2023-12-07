using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveFloorAreaStandardsCommand(string ApplicationId, string HomeTypeId, IReadOnlyCollection<NationallyDescribedSpaceStandardType> NationallyDescribedSpaceStandards)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
