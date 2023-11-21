using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Common.FilePolicies;

public class FilesCountPolicy : IFilePolicy<int>
{
    private const int AllowedNumberOfFiles = 10;

    private readonly string _fieldName;

    public FilesCountPolicy(string fieldName)
    {
        _fieldName = fieldName;
    }

    public void Apply(int value)
    {
        if (value > AllowedNumberOfFiles)
        {
            OperationResult.New().AddValidationError(_fieldName, GenericValidationError.FileCountLimit(AllowedNumberOfFiles)).CheckErrors();
        }
    }
}
