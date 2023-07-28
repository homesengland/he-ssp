using MediatR;
using MediatR.Pipeline;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;
public class RequestPostProcessorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IRequestPostProcessor<TRequest, TResponse>> _postProcessors;

    public RequestPostProcessorBehavior(IEnumerable<IRequestPostProcessor<TRequest, TResponse>> postProcessors)
    {
        _postProcessors = postProcessors;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        await Task.WhenAll(_postProcessors.Select(p => p.Process(request, response, cancellationToken)));

        return response;
    }
}
