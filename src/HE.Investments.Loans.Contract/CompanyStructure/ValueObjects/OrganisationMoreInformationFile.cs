using System.Globalization;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Utils.Constants;

namespace HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;

public class OrganisationMoreInformationFile : ValueObject
{
    public static readonly string[] AllowedExtensions =
    {
        AllowedFileExtension.PDF,
        AllowedFileExtension.DOC,
        AllowedFileExtension.DOCX,
        AllowedFileExtension.JPEG,
        AllowedFileExtension.JPG,
        AllowedFileExtension.RTF,
    };

    public OrganisationMoreInformationFile(string fileName, long fileSize, int maxFileSizeInMb)
    {
        var operationResult = OperationResult.New();
        if (!AllowedExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant()))
        {
            operationResult.AddValidationError(nameof(OrganisationMoreInformationFile), ValidationErrorMessage.FileIncorrectFormat);
        }

        if (fileSize > maxFileSizeInMb * 1024 * 1024)
        {
            operationResult.AddValidationError(
                nameof(OrganisationMoreInformationFile),
                string.Format(CultureInfo.InvariantCulture, ValidationErrorMessage.FileIncorrectSize, maxFileSizeInMb));
        }

        operationResult.CheckErrors();

        FileName = fileName;
    }

    public string FileName { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return FileName;
    }
}
