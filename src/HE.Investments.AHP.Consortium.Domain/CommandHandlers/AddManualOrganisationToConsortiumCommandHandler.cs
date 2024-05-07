extern alias Org;

using System.Diagnostics.CodeAnalysis;
using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using Org::HE.Investments.Organisation.Services;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class AddManualOrganisationToConsortiumCommandHandler : DraftConsortiumCommandHandlerBase<AddManualOrganisationToConsortiumCommand>
{
    private readonly IInvestmentsOrganisationService _organisationService;

    public AddManualOrganisationToConsortiumCommandHandler(
        IConsortiumRepository repository,
        IDraftConsortiumRepository draftConsortiumRepository,
        IInvestmentsOrganisationService organisationService,
        IAccountUserContext accountUserContext)
        : base(repository, draftConsortiumRepository, accountUserContext)
    {
        _organisationService = organisationService;
    }

    protected override Task Perform(IConsortiumEntity consortium, AddManualOrganisationToConsortiumCommand request, CancellationToken cancellationToken)
    {
        var manualOrganisation = ManualOrganisationEntity.Create(
            request.Name,
            request.AddressLine1,
            request.AddressLine2,
            request.TownOrCity,
            request.County,
            request.Postcode);
        var organisation = _organisationService.CreateOrganisation(manualOrganisation);

        consortium.AddMember(organisation, new NotPartOfConsortium(), cancellationToken);

        return Task.FromResult(OperationResult.Success());
    }
}

[SuppressMessage("Maintainability Rules", "SA1400", Justification = "False positive, 'file' scoped classes cannot use accessibility modifiers")]
sealed file record NotPartOfConsortium : IIsPartOfConsortium
{
    public Task<bool> IsPartOfConsortiumForProgramme(ProgrammeId programmeId, OrganisationId organisationId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }
}
