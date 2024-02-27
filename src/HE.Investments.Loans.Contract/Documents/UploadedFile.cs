using HE.Investments.Common.Contract;

namespace HE.Investments.Loans.Contract.Documents;

public record UploadedFile(FileId? Id, string Name, DateTime UploadedOn, string? UploadedBy);
