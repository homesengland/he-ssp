using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Common;

public record UploadedFile(FileId FileId, string FileName, DateTime UploadedOn, string UploadedBy, bool CanBeRemoved);
