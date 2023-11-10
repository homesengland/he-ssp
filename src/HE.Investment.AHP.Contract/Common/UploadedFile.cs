namespace HE.Investment.AHP.Contract.Common;

public record UploadedFile(string FileId, string FileName, DateTime UploadedOn, string UploadedBy, bool CanBeRemoved);
