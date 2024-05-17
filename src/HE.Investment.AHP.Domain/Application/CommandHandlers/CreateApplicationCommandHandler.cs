using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Factories;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, OperationResult<AhpApplicationId>>
{
    private readonly IApplicationRepository _repository;

    private readonly ISiteRepository _siteRepository;

    private readonly IAhpUserContext _ahpUserContext;

    public CreateApplicationCommandHandler(IApplicationRepository repository, ISiteRepository siteRepository, IAhpUserContext ahpUserContext)
    {
        _repository = repository;
        _siteRepository = siteRepository;
        _ahpUserContext = ahpUserContext;
    }

    public async Task<OperationResult<AhpApplicationId>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var name = new ApplicationName(request.Name);
        var account = await _ahpUserContext.GetSelectedAccount();
        if (await _repository.IsNameExist(name, account.SelectedOrganisationId(), cancellationToken))
        {
            throw new FoundException("Name", "There is already an application with this name. Enter a different name");
        }

        var applicationPartners = await GetApplicationPartners(request.SiteId, account, cancellationToken);
        var applicationToCreate = ApplicationEntity.New(
            request.SiteId,
            name,
            new ApplicationTenure(request.Tenure),
            applicationPartners,
            new ApplicationStateFactory(account));
        var application = await _repository.Save(applicationToCreate, account.SelectedOrganisationId(), cancellationToken);

        return new OperationResult<AhpApplicationId>(application.Id);
    }

    private async Task<ApplicationPartners> GetApplicationPartners(SiteId siteId, AhpUserAccount userAccount, CancellationToken cancellationToken)
    {
        if (userAccount.Consortium.HasNoConsortium)
        {
            return ApplicationPartners.ConfirmedPartner(userAccount.SelectedOrganisation());
        }

        var site = await _siteRepository.GetSite(siteId, userAccount, cancellationToken);
        return ApplicationPartners.FromSitePartners(site.SitePartners);
    }
}
