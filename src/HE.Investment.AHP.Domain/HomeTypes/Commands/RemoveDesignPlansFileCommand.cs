namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record RemoveDesignPlansFileCommand(string ApplicationId, string HomeTypeId, string FileId) : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
