namespace HE.Investments.Common.WWW.Models;

public record FileModel(string FileId, string FileName, DateTime UploadedOn, string UploadedBy, bool CanBeRemoved, string RemoveAction);
