using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Domain.ValueObjects;

public abstract class FileValueObject : ValueObject, IDisposable, IAsyncDisposable
{
    private readonly long _fileSize;
    private bool _disposed;

    protected FileValueObject(
        string fieldName,
        string fileName,
        long fileSize,
        int maxFileSizeInMb,
        string[] allowedExtensions,
        string fileFormatValidationErrorMessage,
        Stream fileContent)
    {
        var operationResult = OperationResult.New();

        if (!allowedExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant().TrimStart('.')))
        {
            operationResult.AddValidationError(fieldName, fileFormatValidationErrorMessage);
        }

        if (fileSize > maxFileSizeInMb * 1024 * 1024)
        {
            operationResult.AddValidationError(
                fieldName,
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
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        Dispose(false);
        await FileContent.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                FileContent.Dispose();
            }

            _disposed = true;
        }
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return FileName;
        yield return _fileSize;
    }
}
