namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveExemptionJustificationCommand(
        string ApplicationId,
        string HomeTypeId,
        string? ExemptionJustification)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
