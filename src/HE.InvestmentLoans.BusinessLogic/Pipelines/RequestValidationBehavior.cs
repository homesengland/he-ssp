using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using He.HelpToBuild.Apply.Application.Routing;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Pipelines
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        IServiceProvider _provider;

        public RequestValidationBehavior(IServiceProvider provider, IHttpContextAccessor httpContextAccessor)
        {
            _provider = provider;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            foreach (var prop in typeof(TRequest).GetProperties())
            {
                List<string> rulsets = new List<string>() { "Basic" };

                if (typeof(TRequest).Name.Contains("Create")) { rulsets.Add("Create"); }
                if (typeof(TRequest).Name.Contains("Delete")) { rulsets.Add("Delete"); }
                if (typeof(TRequest).Name.Contains("Update")) { rulsets.Add("Update"); }

                var selector = new RulesetValidatorSelector(rulsets.ToArray());
                var validiator = (IValidator)_provider.GetService(typeof(IValidator<>).MakeGenericType(prop.PropertyType));
                if (validiator != null)
                {
                    var obj = prop.GetValue(request);
                    var context = new ValidationContext<object>(obj, (PropertyChain)null, selector);
                    var result = validiator.Validate(context);
                    if (!result.IsValid)
                    {
                        results.Add(result);
                    }
                }
            }
            if (results.Count() > 0)
            {
                throw new Exceptions.ValidationException(results);
            }
            return next();
        }

       

    }
}
