using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveDesignPlansCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, string? MoreInformation, IList<FileToUpload> Files)
    : ISaveHomeTypeSegmentCommand;
