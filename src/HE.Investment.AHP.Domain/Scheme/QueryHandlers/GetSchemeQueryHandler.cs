using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Application.Mappers;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.AHP.Consortium.Contract;
using MediatR;
using UploadedFile = HE.Investment.AHP.Contract.Common.UploadedFile;

namespace HE.Investment.AHP.Domain.Scheme.QueryHandlers;

public class GetSchemeQueryHandler : IRequestHandler<GetApplicationSchemeQuery, Contract.Scheme.Scheme>
{
    private readonly ISchemeRepository _repository;

    private readonly IAhpUserContext _ahpUserContext;

    public GetSchemeQueryHandler(ISchemeRepository repository, IAhpUserContext ahpUserContext)
    {
        _repository = repository;
        _ahpUserContext = ahpUserContext;
    }

    public async Task<Contract.Scheme.Scheme> Handle(GetApplicationSchemeQuery request, CancellationToken cancellationToken)
    {
        var account = await _ahpUserContext.GetSelectedAccount();
        var entity = await _repository.GetByApplicationId(request.ApplicationId, account, request.IncludeFiles, cancellationToken);

        var (developingPartner, ownerOfTheLand, ownerOfTheHomes, arePartnersConfirmed) = GetPartnerDetails(account);
        return new Contract.Scheme.Scheme(
            ApplicationBasicInfoMapper.Map(entity.Application),
            entity.Status,
            entity.Funding.RequiredFunding,
            entity.Funding.HousesToDeliver,
            developingPartner,
            ownerOfTheLand,
            ownerOfTheHomes,
            arePartnersConfirmed,
            entity.AffordabilityEvidence.Evidence,
            entity.SalesRisk.Value,
            entity.HousingNeeds.MeetingLocalPriorities,
            entity.HousingNeeds.MeetingLocalHousingNeed,
            entity.StakeholderDiscussions.StakeholderDiscussionsDetails.Report,
            CreateFile(entity.StakeholderDiscussions.LocalAuthoritySupportFileContainer),
            !account.Consortium.HasNoConsortium);
    }

    private static UploadedFile? CreateFile(LocalAuthoritySupportFileContainer fileContainer)
    {
        if (fileContainer.File == null)
        {
            return null;
        }

        return new UploadedFile(fileContainer.File.Id, fileContainer.File.Name.Value, fileContainer.File.UploadedOn, fileContainer.File.UploadedBy, true);
    }

    private static (OrganisationDetails? DevelopingPartner, OrganisationDetails? OwnerOfTheLand, OrganisationDetails? OwnerOfTheHomes, bool? ArePartnersConfirmed)
        GetPartnerDetails(AhpUserAccount userAccount)
    {
        // TODO: AB#74674 - return Site/Application partners for consortium
        if (userAccount.Consortium.HasNoConsortium)
        {
            return (null, null, null, null);
        }

        return (new OrganisationDetails("Morelas Developments", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty),
            new OrganisationDetails("Morelas Ltd.", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty),
            new OrganisationDetails("Cactus Developments", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty),
            false);
    }
}
