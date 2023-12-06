using HE.Investment.AHP.Contract.Scheme;
using HE.Investment.AHP.Contract.Scheme.Queries;
using MediatR;
using NuGet.Common;

namespace HE.Investment.AHP.WWW.Utils;

public class CachedSchemeProvider : ISchemeProvider
{
    private readonly IMediator _mediator;
    private AsyncLazy<Scheme>? _scheme;

    public CachedSchemeProvider(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Scheme> Get(GetApplicationSchemeQuery query, CancellationToken cancellationToken)
    {
        if (_scheme == null || query.IncludeFiles)
        {
            _scheme = new AsyncLazy<Scheme>(async () => await _mediator.Send(query, cancellationToken));
        }

        return await _scheme;
    }
}
