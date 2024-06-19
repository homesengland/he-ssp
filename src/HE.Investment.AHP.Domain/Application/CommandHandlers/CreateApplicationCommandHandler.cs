using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Factories;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, OperationResult<AhpApplicationId>>
{
    private readonly IApplicationRepository _repository;

    private readonly ISiteRepository _siteRepository;

    private readonly IConsortiumAccessContext _consortiumUserContext;

    public CreateApplicationCommandHandler(IApplicationRepository repository, ISiteRepository siteRepository, IConsortiumAccessContext consortiumUserContext)
    {
        _repository = repository;
        _siteRepository = siteRepository;
        _consortiumUserContext = consortiumUserContext;
    }

    public async Task<OperationResult<AhpApplicationId>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var name = new ApplicationName(request.Name);
        var account = await _consortiumUserContext.GetSelectedAccount();
        if (await _repository.IsNameExist(name, account.SelectedOrganisationId(), cancellationToken))
        {
            throw new FoundException("Name", "There is already an application with this name. Enter a different name");
        }

        var site = await _siteRepository.GetSite(request.SiteId, account, cancellationToken);
        var applicationPartners = GetApplicationPartners(site, account);
        var applicationToCreate = ApplicationEntity.New(
            site.FrontDoorProjectId,
            request.SiteId,
            name,
            new ApplicationTenure(request.Tenure),
            applicationPartners,
            new ApplicationStateFactory(account));
        var application = await _repository.Save(applicationToCreate, account, cancellationToken);

        return new OperationResult<AhpApplicationId>(application.Id);
    }

    private ApplicationPartners GetApplicationPartners(SiteEntity site, ConsortiumUserAccount userAccount)
    {
        if (userAccount.Consortium.HasNoConsortium)
        {
            return ApplicationPartners.ConfirmedPartner(userAccount.SelectedOrganisation());
        }

        return ApplicationPartners.FromSitePartners(site.SitePartners);
    }
}
