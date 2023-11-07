using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Investment.AHP.BusinessLogic.FinancialDetails.Entities;
using HE.Investment.AHP.BusinessLogic.FinancialDetails.Repositories;
using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;
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
        // temporary mock, this values needs to be taken from some repo
        var isPurchasePriceKnown = true;
        var applicationName = "Some application";

        var financialDetails = new FinancialDetailsEntity(ApplicationID.From(request.ApplicationId), applicationName, null, isPurchasePriceKnown);

        await _financialDetailsRepository.SaveAsync(financialDetails, cancellationToken);

        return OperationResult.Success(new StartFinancialDetailsCommandResult(financialDetails.FinancialDetailsId.Value));
    }
}

