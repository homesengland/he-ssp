using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using MediatR;
using UploadedFile = HE.Investment.AHP.Contract.Common.UploadedFile;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

public class GetDesignPlansQueryHandler : IRequestHandler<GetDesignPlansQuery, DesignPlans>
{
    private readonly IHomeTypeRepository _repository;

    public GetDesignPlansQueryHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DesignPlans> Handle(GetDesignPlansQuery request, CancellationToken cancellationToken)
    {
        var applicationId = new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId);
        var homeType = await _repository.GetById(
            applicationId,
            new HomeTypeId(request.HomeTypeId),
            new[] { HomeTypeSegmentType.DesignPlans },
            cancellationToken);
        var designPlans = homeType.DesignPlans;

        return new DesignPlans(
            homeType.Application.Name.Name,
            homeType.Name.Value,
            designPlans.DesignPrinciples.ToList(),
            designPlans.MoreInformation?.Value,
            designPlans.UploadedFiles.Select(x => MapDesignFile(x, designPlans.CanRemoveDesignFiles)).OrderBy(x => x.UploadedOn).ToList());
    }

    private static UploadedFile MapDesignFile(Common.UploadedFile file, bool canBeRemoved)
    {
        return new UploadedFile(file.Id.Value, file.Name.Value, file.UploadedOn, file.UploadedBy, canBeRemoved);
    }
}
