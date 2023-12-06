using HE.Investment.AHP.Contract.Scheme;
using HE.Investment.AHP.Contract.Scheme.Queries;

namespace HE.Investment.AHP.WWW.Utils;

public interface ISchemeProvider
{
    Task<Scheme> Get(GetApplicationSchemeQuery query, CancellationToken cancellationToken);
}
