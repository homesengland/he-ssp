using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Contract.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Common.WWW.Infrastructure.ErrorHandling;

public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly IModelMetadataProvider _modelMetadataProvider;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IErrorViewPaths _errorViewPaths;
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(
        IModelMetadataProvider modelMetadataProvider,
        IHostEnvironment hostEnvironment,
        IErrorViewPaths errorViewPaths,
        ILogger<ExceptionFilter> logger)
    {
        _modelMetadataProvider = modelMetadataProvider;
        _hostEnvironment = hostEnvironment;
        _errorViewPaths = errorViewPaths;
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
            NotFoundException => _errorViewPaths.PageNotFoundViewPath,
            UnauthorizedAccessException => _errorViewPaths.PageNotFoundViewPath,
            DomainException => _errorViewPaths.ProblemWithTheServiceViewPath,
            _ => _errorViewPaths.ProblemWithTheServiceViewPath,
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
