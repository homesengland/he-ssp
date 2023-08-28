using HE.InvestmentLoans.Common.Models.Others;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.WWW.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.InvestmentLoans.WWW.Filters;

public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly IModelMetadataProvider _modelMetadataProvider;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(IModelMetadataProvider modelMetadataProvider, IHostEnvironment hostEnvironment, ILogger<ExceptionFilter> logger)
    {
        _modelMetadataProvider = modelMetadataProvider;
        _hostEnvironment = hostEnvironment;
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

#pragma warning disable CA1848 // Use the LoggerMessage delegates
        _logger.LogError(exception, "Error occured: {Message}", exception.Message);
#pragma warning restore CA1848 // Use the LoggerMessage delegates

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

    public override Task OnExceptionAsync(ExceptionContext context)
    {
        OnException(context);

        return Task.CompletedTask;
    }

    private string ViewPathFor(Exception exception)
    {
        return exception switch
        {
            NotFoundException => ViewPaths.PageNotFound,
            UnauthorizedAccessException => ViewPaths.PageNotFound,
            DomainException => ViewPaths.ProblemWithTheService,
            _ => ViewPaths.ProblemWithTheService,
        };
    }

    private ErrorModel ErrorModelFrom(Exception exception)
    {
        var errrorModel = exception switch
        {
            DomainException ex => new ErrorModel(ex.Message, ex.ErrorCode, ex.AdditionalData),
            _ => new ErrorModel(exception.Message),
        };

        if (_hostEnvironment.IsDevelopment())
        {
            errrorModel.Message += " " + exception.StackTrace;
        }

        return errrorModel;
    }
}
