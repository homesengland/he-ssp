using HE.Investment.AHP.Domain.Application.Repositories;
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

    private readonly IApplicationRepository _applicationRepository;

    public StartFinancialDetailsCommandHandler(
        IFinancialDetailsRepository financialDetailsRepository,
        IApplicationRepository applicationRepository,
        ILogger<StartFinancialDetailsCommandHandler> logger)
        : base(financialDetailsRepository, logger)
    {
        _financialDetailsRepository = financialDetailsRepository;
        _applicationRepository = applicationRepository;
    }

    public async Task<OperationResult> Handle(StartFinancialDetailsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _financialDetailsRepository.GetById(request.ApplicationId, cancellationToken);
        }
        catch (NotFoundException)
        {
            var applicationBasicInfo = await _applicationRepository.GetApplicationBasicInfo(request.ApplicationId, cancellationToken);
            var financialDetails = new FinancialDetailsEntity(applicationBasicInfo);
            await _financialDetailsRepository.Save(financialDetails, cancellationToken);
        }

        return OperationResult.Success();
    }
}
