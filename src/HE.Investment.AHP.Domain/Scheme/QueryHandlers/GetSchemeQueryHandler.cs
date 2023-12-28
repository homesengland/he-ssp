using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Account.Shared;
using MediatR;
using UploadedFile = HE.Investment.AHP.Contract.Common.UploadedFile;

namespace HE.Investment.AHP.Domain.Scheme.QueryHandlers;

public class GetSchemeQueryHandler : IRequestHandler<GetApplicationSchemeQuery, Contract.Scheme.Scheme>
{
    private readonly ISchemeRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public GetSchemeQueryHandler(ISchemeRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<Contract.Scheme.Scheme> Handle(GetApplicationSchemeQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var entity = await _repository.GetByApplicationId(new(request.ApplicationId), account, request.IncludeFiles, cancellationToken);

        return new Contract.Scheme.Scheme(
            entity.Application.Id.Value,
            entity.Application.Name.Name,
            entity.Application.Tenure?.Value,
            entity.Status,
            entity.Funding.RequiredFunding,
            entity.Funding.HousesToDeliver,
            entity.AffordabilityEvidence.Evidence,
            entity.SalesRisk.Value,
            entity.HousingNeeds.MeetingLocalPriorities,
            entity.HousingNeeds.MeetingLocalHousingNeed,
            entity.StakeholderDiscussions.StakeholderDiscussionsDetails.Report,
            CreateFile(entity.StakeholderDiscussions.LocalAuthoritySupportFileContainer));
    }

    private static UploadedFile? CreateFile(LocalAuthoritySupportFileContainer fileContainer)
    {
        if (fileContainer.File == null)
        {
            return null;
        }

        return new UploadedFile(fileContainer.File.Id.Value, fileContainer.File.Name.Value, fileContainer.File.UploadedOn, fileContainer.File.UploadedBy, true);
    }
}
