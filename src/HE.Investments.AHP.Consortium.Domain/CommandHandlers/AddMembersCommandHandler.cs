using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class AddMembersCommandHandler : IRequestHandler<AddMembersCommand, OperationResult>
{
    private readonly IDraftConsortiumRepository _draftConsortiumRepository;

    private readonly IConsortiumRepository _consortiumRepository;

    private readonly IAccountUserContext _accountUserContext;

    public AddMembersCommandHandler(
        IDraftConsortiumRepository draftConsortiumRepository,
        IConsortiumRepository consortiumRepository,
        IAccountUserContext accountUserContext)
    {
        _draftConsortiumRepository = draftConsortiumRepository;
        _consortiumRepository = consortiumRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(AddMembersCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var draftConsortium = _draftConsortiumRepository.Get(request.ConsortiumId, userAccount, throwException: true)!;
        var consortium = await _consortiumRepository.GetConsortium(request.ConsortiumId, userAccount, cancellationToken);

        if (consortium.AddMembersFromDraft(draftConsortium, request.AreAllMembersAdded))
        {
            await _consortiumRepository.Save(consortium, userAccount, cancellationToken);
            _draftConsortiumRepository.Delete(draftConsortium, userAccount);
        }

        return OperationResult.Success();
    }
}
