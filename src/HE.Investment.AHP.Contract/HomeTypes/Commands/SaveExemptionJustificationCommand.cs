using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveExemptionJustificationCommand(
        AhpApplicationId ApplicationId,
        HomeTypeId HomeTypeId,
        string? ExemptionJustification)
    : ISaveHomeTypeSegmentCommand;
