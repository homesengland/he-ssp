using HE.Investment.AHP.Contract.PrefillData;
using HE.Investment.AHP.Contract.PrefillData.Queries;
using HE.Investment.AHP.Domain.PrefillData.Repositories;
using MediatR;

namespace HE.Investment.AHP.Domain.PrefillData.QueryHandlers;

public class GetNewAhpSitePrefillDataQueryHandler : IRequestHandler<GetNewAhpSitePrefillDataQuery, NewAhpSitePrefillData>
{
    private readonly IAhpPrefillDataRepository _prefillDataRepository;

    public GetNewAhpSitePrefillDataQueryHandler(IAhpPrefillDataRepository prefillDataRepository)
    {
        _prefillDataRepository = prefillDataRepository;
    }

    public async Task<NewAhpSitePrefillData> Handle(GetNewAhpSitePrefillDataQuery request, CancellationToken cancellationToken)
    {
        var prefillData = await _prefillDataRepository.GetAhpSitePrefillData(
            request.ProjectId,
            request.SiteId,
            cancellationToken);

        return new NewAhpSitePrefillData(prefillData.SiteName);
    }
}
