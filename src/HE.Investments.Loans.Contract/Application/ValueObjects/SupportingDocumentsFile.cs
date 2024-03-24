using HE.Investments.Common.Constants;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Messages;

namespace HE.Investments.Loans.Contract.Application.ValueObjects;

public sealed class SupportingDocumentsFile : FileValueObject
{
    public static readonly string[] SharePointAllowedExtensions = AllowedExtensions.AllAllowedSharePointExtensions;

    public SupportingDocumentsFile(string fileName, long fileSize, int maxFileSizeInMb, Stream fileContent)
        : base(
            nameof(SupportingDocumentsFile),
            fileName,
            fileSize,
            maxFileSizeInMb,
            SharePointAllowedExtensions,
            ValidationErrorMessage.FileFormatNotSupported,
            fileContent)
    {
    }
}
