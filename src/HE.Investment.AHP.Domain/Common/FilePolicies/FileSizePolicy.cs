using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Common.FilePolicies;

public class FileSizePolicy : IFilePolicy<FileSize>
{
    private const int MaxFileSizeInMegabytes = 20;

    private readonly string _fieldName;

    public FileSizePolicy(string fieldName)
    {
        _fieldName = fieldName;
    }

    public void Apply(FileSize value)
    {
        if (value > FileSize.FromMegabytes(MaxFileSizeInMegabytes))
        {
            OperationResult.New()
                .AddValidationError(_fieldName, GenericValidationError.FileTooBig(MaxFileSizeInMegabytes))
                .CheckErrors();
        }
    }
}
