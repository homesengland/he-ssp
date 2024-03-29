using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record RemoveDesignPlansFileCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, FileId FileId) : ISaveHomeTypeSegmentCommand;
