using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.Contract.Application.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.CommandHandlers;

public class ConfirmInformationAgreementCommandHandler : IRequestHandler<ConfirmInformationAgreementCommand, OperationResult>
{
    public Task<OperationResult> Handle(ConfirmInformationAgreementCommand request, CancellationToken cancellationToken)
    {
        InformationAgreement.FromString(request.InformationAgreement);
        return Task.FromResult(OperationResult.Success());
    }
}
