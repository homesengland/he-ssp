using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

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
        FinancialDetailsEntity financialDetails;
        try
        {
            financialDetails = await _financialDetailsRepository.GetById(request.ApplicationId, cancellationToken);
        }
        catch (NotFoundException)
        {
            financialDetails = new FinancialDetailsEntity(request.ApplicationId, request.ApplicationName);
            await _financialDetailsRepository.Save(financialDetails, cancellationToken);
        }

        return OperationResult.Success();
    }
}
