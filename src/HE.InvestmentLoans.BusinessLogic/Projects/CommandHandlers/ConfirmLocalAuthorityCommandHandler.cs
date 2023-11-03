using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
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
        await _localAuthorityRepository.AssignLocalAuthority(request.LoanApplicationId, request.ProjectId, request.LocalAuthorityId, cancellationToken);

        return OperationResult.Success();
    }
}
