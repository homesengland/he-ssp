using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveTypologyLocationAndDesignCommand(
        AhpApplicationId ApplicationId,
        string HomeTypeId,
        string? TypologyLocationAndDesign)
    : ISaveHomeTypeSegmentCommand;
