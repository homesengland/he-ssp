using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record RemoveDesignPlansFileCommand(AhpApplicationId ApplicationId, string HomeTypeId, string FileId) : ISaveHomeTypeSegmentCommand;
