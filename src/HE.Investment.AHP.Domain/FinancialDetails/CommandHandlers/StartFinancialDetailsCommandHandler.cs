using HE.Investment.AHP.BusinessLogic.FinancialDetails.CommandHandlers;
using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

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
        var financialDetails = new FinancialDetailsEntity(FinancialSchemeId.From(request.FinancialSchemeId));
        await _financialDetailsRepository.SaveAsync(financialDetails, cancellationToken);

        return OperationResult.Success(new StartFinancialDetailsCommandResult(financialDetails.Id.Value));
    }
}
