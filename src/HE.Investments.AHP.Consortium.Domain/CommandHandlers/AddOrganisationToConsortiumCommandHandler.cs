extern alias Org;

using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using Org::HE.Investments.Organisation.Services;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class AddOrganisationToConsortiumCommandHandler : DraftConsortiumCommandHandlerBase<AddOrganisationToConsortiumCommand>
{
    private readonly IConsortiumRepository _repository;

    private readonly IInvestmentsOrganisationService _organisationService;

    public AddOrganisationToConsortiumCommandHandler(
        IConsortiumRepository repository,
        IDraftConsortiumRepository draftConsortiumRepository,
        IInvestmentsOrganisationService organisationService,
        IAccountUserContext accountUserContext)
        : base(repository, draftConsortiumRepository, accountUserContext)
    {
        _repository = repository;
        _organisationService = organisationService;
    }

    protected override async Task Perform(IConsortiumEntity consortium, AddOrganisationToConsortiumCommand request, CancellationToken cancellationToken)
    {
        if (request.SelectedMember.IsNotProvided())
        {
            OperationResult.ThrowValidationError(
                nameof(request.SelectedMember),
                ValidationErrorMessage.MustBeSelected("organisation"));
        }

        var organisation = await _organisationService.GetOrganisation(request.SelectedMember!, cancellationToken);
        await consortium.AddMember(organisation, _repository, cancellationToken);
    }
}
