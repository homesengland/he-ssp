using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.Contract.Application.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.CommandHandlers;

public class ConfirmInformationAgreementCommandHandler : IRequestHandler<ConfirmInformationAgreementCommand, OperationResult>
{
    private readonly ILogger<ConfirmInformationAgreementCommandHandler> _logger;

    public ConfirmInformationAgreementCommandHandler(ILogger<ConfirmInformationAgreementCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task<OperationResult> Handle(ConfirmInformationAgreementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            InformationAgreement.FromString(request.InformationAgreement);
            return Task.FromResult(OperationResult.Success());
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return Task.FromResult(domainValidationException.OperationResult);
        }
    }
}
