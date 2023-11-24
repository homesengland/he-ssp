namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveTypologyLocationAndDesignCommand(
        string ApplicationId,
        string HomeTypeId,
        string? TypologyLocationAndDesign)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
