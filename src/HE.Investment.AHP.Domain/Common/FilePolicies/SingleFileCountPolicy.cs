using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Common.FilePolicies;

public class SingleFileCountPolicy : IFilePolicy<int>
{
    private const int AllowedNumberOfFiles = 1;

    private readonly string _fieldName;

    public SingleFileCountPolicy(string fieldName)
    {
        _fieldName = fieldName;
    }

    public void Apply(int value, OperationResult operationResult)
    {
        if (value > AllowedNumberOfFiles)
        {
            operationResult.AddValidationError(_fieldName, GenericValidationError.FileCountLimit(AllowedNumberOfFiles));
        }
    }
}
