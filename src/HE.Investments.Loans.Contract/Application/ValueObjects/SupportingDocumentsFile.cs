using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Messages;

namespace HE.Investments.Loans.Contract.Application.ValueObjects;

public sealed class SupportingDocumentsFile : FileValueObject
{
    public static readonly string SharePointAllowedExtensions = "AllFileTypesAreAllowed";

    public SupportingDocumentsFile(string fileName, long fileSize, int maxFileSizeInMb, Stream fileContent)
        : base(
            nameof(SupportingDocumentsFile),
            fileName,
            fileSize,
            maxFileSizeInMb,
            null,
            ValidationErrorMessage.FileFormatNotSupported,
            fileContent)
    {
    }
}
