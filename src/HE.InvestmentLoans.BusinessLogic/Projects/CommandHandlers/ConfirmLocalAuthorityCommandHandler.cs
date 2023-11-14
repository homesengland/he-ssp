using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ConfirmLocalAuthorityCommandHandler : IRequestHandler<ConfirmLocalAuthorityCommand, OperationResult>
{
    private readonly ILocalAuthorityRepository _localAuthorityRepository;

    public ConfirmLocalAuthorityCommandHandler(ILocalAuthorityRepository localAuthorityRepository)
    {
        _localAuthorityRepository = localAuthorityRepository;
    }

    public async Task<OperationResult> Handle(ConfirmLocalAuthorityCommand request, CancellationToken cancellationToken)
    {
        await _localAuthorityRepository.AssignLocalAuthority(request.LoanApplicationId, request.ProjectId, request.LocalAuthority, cancellationToken);

        return OperationResult.Success();
    }
}
