using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveHappiDesignPrinciplesCommand(string ApplicationId, string HomeTypeId, IReadOnlyCollection<HappiDesignPrincipleType> DesignPrinciples)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
