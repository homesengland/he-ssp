using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

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
        try
        {
            await _financialDetailsRepository.GetById(request.ApplicationId, cancellationToken);
        }
        catch (NotFoundException)
        {
            var financialDetails = new FinancialDetailsEntity(request.ApplicationId, request.ApplicationName);
            await _financialDetailsRepository.Save(financialDetails, cancellationToken);
        }

        return OperationResult.Success();
    }
}
