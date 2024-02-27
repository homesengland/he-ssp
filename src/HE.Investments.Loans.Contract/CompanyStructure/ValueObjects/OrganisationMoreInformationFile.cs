using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;

namespace HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;

public sealed class OrganisationMoreInformationFile : ValueObject, IDisposable, IAsyncDisposable
{
    public static readonly string[] AllowedExtensions = { "pdf", "doc", "docx", "jpeg", "jpg", "rtf" };

    private readonly long _fileSize;

    public OrganisationMoreInformationFile(string fileName, long fileSize, int maxFileSizeInMb, Stream fileContent)
    {
        var operationResult = OperationResult.New();
        if (!AllowedExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant().TrimStart('.')))
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

        _fileSize = fileSize;
        FileName = fileName;
        FileContent = fileContent;
    }

    public string FileName { get; }

    public Stream FileContent { get; }

    public void Dispose()
    {
        FileContent.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await FileContent.DisposeAsync();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return FileName;
        yield return _fileSize;
    }
}
