using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using MediatR;
using UploadedFile = HE.Investment.AHP.Contract.Common.UploadedFile;

namespace HE.Investment.AHP.Domain.Scheme.QueryHandlers;

public class GetSchemeQueryHandler : IRequestHandler<GetApplicationSchemeQuery, Contract.Scheme.Scheme>
{
    private readonly ISchemeRepository _repository;

    public GetSchemeQueryHandler(ISchemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Contract.Scheme.Scheme> Handle(GetApplicationSchemeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByApplicationId(new(request.ApplicationId), cancellationToken);

        return new Contract.Scheme.Scheme(
            entity.Application.Id.Value,
            entity.Application.Name.Name,
            entity.Application.Tenure?.Value,
            entity.Status,
            entity.Funding.RequiredFunding,
            entity.Funding.HousesToDeliver,
            entity.AffordabilityEvidence.Evidence,
            entity.SalesRisk.Value,
            entity.HousingNeeds.TypeAndTenureJustification,
            entity.HousingNeeds.SchemeAndProposalJustification,
            entity.StakeholderDiscussions.Report,
            CreateFile(entity.StakeholderDiscussionsFiles));
    }

    private static UploadedFile? CreateFile(StakeholderDiscussionsFiles files)
    {
        if (!files.UploadedFiles.Any())
        {
            return null;
        }

        var file = files.UploadedFiles.First();

        return new UploadedFile(file.Id.Value, file.Name.Value, file.UploadedOn, file.UploadedBy, true);
    }
}
