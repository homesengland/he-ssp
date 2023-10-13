using System.Globalization;
using System.Threading;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;
using HE.Investments.DocumentService.Models.File;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

public class OrganisationMoreInformationFile : ValueObject
{
    private readonly string[] _allowedExtensions =
    {
        AllowedFileExtension.PDF,
        AllowedFileExtension.DOC,
        AllowedFileExtension.DOCX,
        AllowedFileExtension.JPEG,
        AllowedFileExtension.JPG,
        AllowedFileExtension.RTF,
    };

    public OrganisationMoreInformationFile(FileData file, int maxFileSizeInMb)
    {
        var operationResult = OperationResult.New();
        if (!_allowedExtensions.Contains(Path.GetExtension(file.Name).ToLowerInvariant()))
        {
            operationResult.AddValidationError(nameof(OrganisationMoreInformationFile), ValidationErrorMessage.FileIncorrectFormat);
        }

        if (file.Data.Length > maxFileSizeInMb * 1024 * 1024)
        {
            operationResult.AddValidationError(
                nameof(OrganisationMoreInformationFile),
                string.Format(CultureInfo.InvariantCulture, ValidationErrorMessage.FileIncorrectSize, maxFileSizeInMb));
        }

        operationResult.CheckErrors();
        File = file;
    }

    public FileData File { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return File;
    }
}
