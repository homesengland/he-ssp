using HE.Investments.Common.Contract.Validators;
using HE.Investments.Organisation.Contract.Commands;
using HE.Investments.Organisation.Entities;
using HE.Investments.Organisation.Services;
using HE.Investments.Organisation.ValueObjects;
using MediatR;

namespace HE.Investments.Organisation.CommandHandlers;

internal sealed class CreateManualOrganisationCommandHandler : IRequestHandler<CreateManualOrganisationCommand, OperationResult<InvestmentsOrganisation>>
{
    private readonly IInvestmentsOrganisationService _organisationService;

    public CreateManualOrganisationCommandHandler(IInvestmentsOrganisationService organisationService)
    {
        _organisationService = organisationService;
    }

    public Task<OperationResult<InvestmentsOrganisation>> Handle(CreateManualOrganisationCommand request, CancellationToken cancellationToken)
    {
        var manualOrganisation = ManualOrganisationEntity.Create(
            request.Name,
            request.AddressLine1,
            request.AddressLine2,
            request.TownOrCity,
            request.County,
            request.Postcode);
        var organisation = _organisationService.CreateOrganisation(manualOrganisation);

        return Task.FromResult(OperationResult.Success(organisation));
    }
}
