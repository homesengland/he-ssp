using HE.InvestmentLoans.Common.Models.Others;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.WWW.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.InvestmentLoans.WWW.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly IModelMetadataProvider _modelMetadataProvider;
    private readonly IHostEnvironment _hostEnvironment;

    public ExceptionFilter(IModelMetadataProvider modelMetadataProvider, IHostEnvironment hostEnvironment)
    {
        _modelMetadataProvider = modelMetadataProvider;
        _hostEnvironment = hostEnvironment;
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        var result = new ViewResult
        {
            ViewName = ViewPathFor(exception),
            ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState)
            {
                Model = ErrorModelFrom(exception),
            },
        };

        context.ExceptionHandled = true;
        context.Result = result;
    }

    private string ViewPathFor(Exception exception)
    {
        return exception switch
        {
            NotFoundException => ViewPaths.PageNotFound,
            _ => ViewPaths.ProblemWithTheService,
        };
    }

    private ErrorModel ErrorModelFrom(Exception exception)
    {
        return _hostEnvironment.IsDevelopment() ? new ErrorModel(exception.Message, exception.StackTrace) : new ErrorModel(exception.Message);
    }
}
