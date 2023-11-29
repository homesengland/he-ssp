using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveCustomBuildPropertyCommand(string ApplicationId, string HomeTypeId, YesNoType CustomBuild)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
