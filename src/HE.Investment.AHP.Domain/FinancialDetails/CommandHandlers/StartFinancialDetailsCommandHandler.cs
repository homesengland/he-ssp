using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Investment.AHP.BusinessLogic.FinancialDetails.Entities;
using HE.Investment.AHP.BusinessLogic.FinancialDetails.Repositories;
using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace HE.Investment.AHP.BusinessLogic.FinancialDetails.CommandHandlers;
public class StartFinancialDetailsCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<StartFinancialDetailsCommand, OperationResult<StartFinancialDetailsCommandResult>>
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    public StartFinancialDetailsCommandHandler(IFinancialDetailsRepository financialDetailsRepository, ILogger<StartFinancialDetailsCommandHandler> logger)
        : base(financialDetailsRepository, logger)
    {
        _financialDetailsRepository = financialDetailsRepository;
    }

    public async Task<OperationResult<StartFinancialDetailsCommandResult>> Handle(StartFinancialDetailsCommand request, CancellationToken cancellationToken)
    {
        var financialDetails = new FinancialDetailsEntity();
        await _financialDetailsRepository.SaveAsync(financialDetails, cancellationToken);

        return OperationResult.Success(new StartFinancialDetailsCommandResult(financialDetails.Id.Value));
    }
}

