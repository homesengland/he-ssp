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
public class CreateFinancialDetailsCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<CreateFinancialDetailsCommand, OperationResult<CreateFinancialDetailsCommandResult>>
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;


    public CreateFinancialDetailsCommandHandler(IFinancialDetailsRepository financialDetailsRepository, ILogger<CreateFinancialDetailsCommandHandler> logger)
        : base(financialDetailsRepository, logger)
    {
        _financialDetailsRepository = financialDetailsRepository;
    }

    public async Task<OperationResult<CreateFinancialDetailsCommandResult>> Handle(CreateFinancialDetailsCommand request, CancellationToken cancellationToken)
    {
        var financialDetails = new FinancialDetailsEntity();
        await _financialDetailsRepository.SaveAsync(financialDetails, cancellationToken);

        return OperationResult.Success(new CreateFinancialDetailsCommandResult(financialDetails.Id.Value));
    }
}

