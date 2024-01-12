using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Common;

public record UploadedFile(FileId Id, FileName Name, DateTime UploadedOn, string UploadedBy);
