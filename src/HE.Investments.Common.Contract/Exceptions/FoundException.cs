using HE.Investments.Common.Contract.Validators;

namespace HE.Investments.Common.Contract.Exceptions;

public class FoundException : DomainValidationException
{
    public FoundException(string name, string message)
        : base(new OperationResult([new ErrorItem(name, message)]))
    {
    }
}
