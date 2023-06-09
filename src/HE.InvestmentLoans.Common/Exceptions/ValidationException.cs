using FluentValidation.Results;

namespace HE.InvestmentLoans.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(List<ValidationResult> results)
        : base(string.Join(';', results.Select(item => item.ToString()).ToArray()))
    {
        Results = results;
    }

    public List<ValidationResult> Results { get; set; }
}
