using HE.Investments.Common.Validators;

namespace HE.Investments.Common.Exceptions;

public class FoundException : DomainValidationException
{
    public FoundException(string name, string message)
        : base(new OperationResult(new[] { new ErrorItem(name, message) }))
    {
    }
}
