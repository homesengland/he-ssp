using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class CreateConsortiumCommandHandler : IRequestHandler<CreateConsortiumCommand, OperationResult<ConsortiumId>>
{
    private readonly IConsortiumRepository _consortiumRepository;

    private readonly IAccountUserContext _accountUserContext;

    public CreateConsortiumCommandHandler(IConsortiumRepository consortiumRepository, IAccountUserContext accountUserContext)
    {
        _consortiumRepository = consortiumRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult<ConsortiumId>> Handle(CreateConsortiumCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ProgrammeId))
        {
            OperationResult.ThrowValidationError("SelectedProgrammeId", "Please select what programme the consortium is related to");
        }

        var programme = new ProgrammeSlim(ProgrammeId.From(request.ProgrammeId), "AHP CME");
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var organisation = userAccount.SelectedOrganisation();
        var leadPartner = new ConsortiumMember(organisation.OrganisationId, organisation.RegisteredCompanyName, ConsortiumMemberStatus.Active);

        var consortium = await ConsortiumEntity.New(programme, leadPartner, _consortiumRepository);

        await _consortiumRepository.Save(consortium, userAccount, cancellationToken);
        return new OperationResult<ConsortiumId>(consortium.Id);
    }
}
