using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Application.Mappers;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Shared.UserContext;
using HE.Investments.Organisation.ValueObjects;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.QueryHandlers;

public class GetSchemeQueryHandler : IRequestHandler<GetApplicationSchemeQuery, Contract.Scheme.Scheme>
{
    private readonly ISchemeRepository _repository;

    private readonly IConsortiumUserContext _consortiumUserContext;

    public GetSchemeQueryHandler(ISchemeRepository repository, IConsortiumUserContext consortiumUserContext)
    {
        _repository = repository;
        _consortiumUserContext = consortiumUserContext;
    }

    public async Task<Contract.Scheme.Scheme> Handle(GetApplicationSchemeQuery request, CancellationToken cancellationToken)
    {
        var account = await _consortiumUserContext.GetSelectedAccount();
        var entity = await _repository.GetByApplicationId(request.ApplicationId, account, request.IncludeFiles, cancellationToken);

        return new Contract.Scheme.Scheme(
            ApplicationBasicInfoMapper.Map(entity.Application),
            entity.Status,
            entity.Funding.RequiredFunding,
            entity.Funding.HousesToDeliver,
            MapPartner(entity.ApplicationPartners.DevelopingPartner),
            MapPartner(entity.ApplicationPartners.OwnerOfTheLand),
            MapPartner(entity.ApplicationPartners.OwnerOfTheHomes),
            entity.ApplicationPartners.ArePartnersConfirmed,
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

    private static OrganisationDetails MapPartner(InvestmentsOrganisation organisation)
    {
        return new OrganisationDetails(organisation.Name, string.Empty, string.Empty, string.Empty, string.Empty, organisation.Id.ToString());
    }
}
