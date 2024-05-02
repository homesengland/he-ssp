using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Messages;

namespace HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;

public sealed class OrganisationMoreInformationFile : FileValueObject
{
    public static readonly string[] AllowedExtensions = ["pdf", "doc", "docx", "jpeg", "jpg", "rtf"];

    public OrganisationMoreInformationFile(string fileName, long fileSize, int maxFileSizeInMb, Stream fileContent)
        : base(
            nameof(OrganisationMoreInformationFile),
            fileName,
            fileSize,
            maxFileSizeInMb,
            AllowedExtensions,
            ValidationErrorMessage.FileIncorrectFormat,
            fileContent)
    {
    }
}
