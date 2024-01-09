using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Common.FilePolicies;

public class FileSizePolicy : IFilePolicy<FileSize>
{
    private readonly string _fieldName;
    private readonly FileSize _maxFileSize;

    public FileSizePolicy(string fieldName, FileSize maxFileSize)
    {
        _fieldName = fieldName;
        _maxFileSize = maxFileSize;
    }

    public void Apply(FileSize value)
    {
        if (value > _maxFileSize)
        {
            OperationResult.New()
                .AddValidationError(_fieldName, GenericValidationError.FileTooBig(_maxFileSize.Megabytes))
                .CheckErrors();
        }
    }
}
