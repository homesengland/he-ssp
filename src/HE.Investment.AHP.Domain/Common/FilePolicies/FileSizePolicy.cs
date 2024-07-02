using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Common.FilePolicies;

public class FileSizePolicy : IFilePolicy<FileSize>
{
    private readonly string _fieldName;
    private readonly FileSize _maxFileSize;
    private readonly FileName _fileName;

    public FileSizePolicy(string fieldName, FileName fileName, FileSize maxFileSize)
    {
        _fieldName = fieldName;
        _maxFileSize = maxFileSize;
        _fileName = fileName;
    }

    public void Apply(FileSize value, OperationResult operationResult)
    {
        if (value > _maxFileSize)
        {
            operationResult.Aggregate(() =>
                throw new DomainValidationException(_fieldName, GenericValidationError.FileTooBig(_fileName.Value, _maxFileSize.Megabytes)));
        }
    }
}
