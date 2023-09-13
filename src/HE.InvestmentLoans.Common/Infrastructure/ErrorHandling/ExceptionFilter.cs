using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.Common.Infrastructure.ErrorHandling;

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

    [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "We dont need high performace logging.")]
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        _logger.LogError(exception, "Error occured: {Message}", exception.Message);

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
            NotFoundException => ErrorViewPaths.PageNotFound,
            UnauthorizedAccessException => ErrorViewPaths.PageNotFound,
            DomainException => ErrorViewPaths.ProblemWithTheService,
            _ => ErrorViewPaths.ProblemWithTheService,
        };
    }

    private ErrorModel ErrorModelFrom(Exception exception)
    {
        var errorModel = exception switch
        {
            DomainException ex => new ErrorModel(ex.Message, ex.ErrorCode, ex.AdditionalData),
            _ => new ErrorModel(exception.Message),
        };

        if (_hostEnvironment.IsDevelopment())
        {
            errorModel.Message += " " + exception.StackTrace;
        }

        return errorModel;
    }
}
