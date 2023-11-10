using HE.Investment.AHP.Domain.Common;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveDesignPlansCommand(string ApplicationId, string HomeTypeId, string? MoreInformation, IList<FileToUpload> Files)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
