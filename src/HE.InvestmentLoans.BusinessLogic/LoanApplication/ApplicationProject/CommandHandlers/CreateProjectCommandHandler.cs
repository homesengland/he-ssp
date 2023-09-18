using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.CommandHandlers;
public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, OperationResult<Guid>>
{
    private ILoanApplicationRepository _loanApplicationRepository;

    public CreateProjectCommandHandler(ILoanApplicationRepository loanApplicationRepository)
    {
        _loanApplicationRepository = loanApplicationRepository;
    }

    public async Task<OperationResult<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        //var loanApplication = await _loanApplicationRepository.GetLoanApplication();

        return null;
    }
}
