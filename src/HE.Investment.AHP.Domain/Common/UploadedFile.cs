using HE.Investment.AHP.Domain.Common.ValueObjects;

namespace HE.Investment.AHP.Domain.Common;

public record UploadedFile(FileId Id, FileName Name, DateTime UploadedOn, string UploadedBy);
