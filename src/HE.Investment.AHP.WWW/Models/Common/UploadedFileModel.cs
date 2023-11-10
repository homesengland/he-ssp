namespace HE.Investment.AHP.WWW.Models.Common;

public record UploadedFileModel(string FileId, string FileName, DateTime UploadedOn, string UploadedBy, bool CanBeRemoved);
