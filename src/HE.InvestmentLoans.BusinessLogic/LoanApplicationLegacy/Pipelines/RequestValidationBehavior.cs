using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using MediatR;
using ValidationException = HE.InvestmentLoans.Common.Exceptions.ValidationException;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Pipelines;

public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IServiceProvider _provider;

    public RequestValidationBehavior(IServiceProvider provider)
    {
        _provider = provider;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var results = new List<ValidationResult>();

        foreach (var prop in typeof(TRequest).GetProperties())
        {
            var rulsets = new List<string>() { "Basic" };

            if (typeof(TRequest).Name.Contains("Create"))
            {
                rulsets.Add("Create");
            }

            if (typeof(TRequest).Name.Contains("Delete"))
            {
                rulsets.Add("Delete");
            }

            if (typeof(TRequest).Name.Contains("Update"))
            {
                rulsets.Add("Update");
            }

            var selector = new RulesetValidatorSelector(rulsets.ToArray());
            if (_provider.GetService(typeof(IValidator<>).MakeGenericType(prop.PropertyType)) is IValidator validiator)
            {
                var obj = prop.GetValue(request);
                var context = new ValidationContext<object>(obj!, null, selector);
                var result = validiator.Validate(context);
                if (!result.IsValid)
                {
                    results.Add(result);
                }
            }
        }

        if (results.Count > 0)
        {
            throw new ValidationException(results);
        }

        return next();
    }
}
