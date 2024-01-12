using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveTypologyLocationAndDesignCommand(
        AhpApplicationId ApplicationId,
        HomeTypeId HomeTypeId,
        string? TypologyLocationAndDesign)
    : ISaveHomeTypeSegmentCommand;
