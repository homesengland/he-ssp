using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Contract.FinancialDetails.Models;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Contract.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;
public class StartFinancialDetailsCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<StartFinancialDetailsCommand, OperationResult>
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    public StartFinancialDetailsCommandHandler(IFinancialDetailsRepository financialDetailsRepository, ILogger<StartFinancialDetailsCommandHandler> logger)
        : base(financialDetailsRepository, logger)
    {
        _financialDetailsRepository = financialDetailsRepository;
    }

    public async Task<OperationResult> Handle(StartFinancialDetailsCommand request, CancellationToken cancellationToken)
    {
        // temporary mock, this values needs to be taken from some repo
        var isPurchasePriceKnown = true;
        FinancialDetailsEntity financialDetails;
        try
        {
            financialDetails = await _financialDetailsRepository.GetById(ApplicationId.From(request.ApplicationId), cancellationToken);
        }
        catch (NotFoundException)
        {
            financialDetails = new FinancialDetailsEntity(ApplicationId.From(request.ApplicationId), request.ApplicationName, isPurchasePriceKnown);
            await _financialDetailsRepository.SaveAsync(financialDetails, cancellationToken);
        }

        return OperationResult.Success();
    }
}
