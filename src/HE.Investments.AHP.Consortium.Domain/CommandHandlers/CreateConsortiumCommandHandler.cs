using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Programme.Contract.Queries;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class CreateConsortiumCommandHandler : IRequestHandler<CreateConsortiumCommand, OperationResult<ConsortiumId>>
{
    private readonly IConsortiumRepository _consortiumRepository;

    private readonly IDraftConsortiumRepository _draftConsortiumRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IMediator _mediator;

    public CreateConsortiumCommandHandler(
        IConsortiumRepository consortiumRepository,
        IDraftConsortiumRepository draftConsortiumRepository,
        IAccountUserContext accountUserContext,
        IMediator mediator)
    {
        _consortiumRepository = consortiumRepository;
        _draftConsortiumRepository = draftConsortiumRepository;
        _accountUserContext = accountUserContext;
        _mediator = mediator;
    }

    public async Task<OperationResult<ConsortiumId>> Handle(CreateConsortiumCommand request, CancellationToken cancellationToken)
    {
        if (request.ProgrammeId.IsNotProvided())
        {
            OperationResult.ThrowValidationError("SelectedProgrammeId", "Please select what programme the consortium is related to");
        }

        var programme = await _mediator.Send(new GetProgrammeQuery(request.ProgrammeId!), cancellationToken);
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var organisation = userAccount.SelectedOrganisation();
        var leadPartner = new ConsortiumMember(organisation.OrganisationId, organisation.RegisteredCompanyName, ConsortiumMemberStatus.Active);

        var consortium = await ConsortiumEntity.New(programme, leadPartner, _consortiumRepository);

        await _consortiumRepository.Save(consortium, userAccount, cancellationToken);
        _draftConsortiumRepository.Create(consortium, userAccount);

        return new OperationResult<ConsortiumId>(consortium.Id);
    }
}
