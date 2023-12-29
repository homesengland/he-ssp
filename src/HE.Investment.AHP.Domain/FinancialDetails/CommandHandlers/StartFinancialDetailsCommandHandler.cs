using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;

public class StartFinancialDetailsCommandHandler : IRequestHandler<StartFinancialDetailsCommand, OperationResult>
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    private readonly IApplicationRepository _applicationRepository;

    private readonly IAccountUserContext _accountUserContext;

    public StartFinancialDetailsCommandHandler(
        IFinancialDetailsRepository financialDetailsRepository,
        IApplicationRepository applicationRepository,
        IAccountUserContext accountUserContext)
    {
        _financialDetailsRepository = financialDetailsRepository;
        _applicationRepository = applicationRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(StartFinancialDetailsCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();

        try
        {
            await _financialDetailsRepository.GetById(request.ApplicationId, account, cancellationToken);
        }
        catch (NotFoundException)
        {
            var applicationBasicInfo = await _applicationRepository.GetApplicationBasicInfo(request.ApplicationId, account, cancellationToken);
            var financialDetails = new FinancialDetailsEntity(applicationBasicInfo);
            await _financialDetailsRepository.Save(financialDetails, account.SelectedOrganisationId(), cancellationToken);
        }

        return OperationResult.Success();
    }
}
