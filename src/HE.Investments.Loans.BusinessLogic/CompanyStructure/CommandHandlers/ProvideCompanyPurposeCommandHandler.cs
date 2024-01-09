using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideCompanyPurposeCommandHandler : CompanyStructureBaseCommandHandler, IRequestHandler<ProvideCompanyPurposeCommand, OperationResult>
{
    public ProvideCompanyPurposeCommandHandler(ICompanyStructureRepository companyStructureRepository, ILoanApplicationRepository loanApplicationRepository, IAccountUserContext loanUserContext, ILogger<CompanyStructureBaseCommandHandler> logger)
        : base(companyStructureRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideCompanyPurposeCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            companyStructure =>
            {
                var companyPurpose = request.CompanyPurpose.IsProvided() ? CompanyPurpose.FromString(request.CompanyPurpose!) : null;
                companyStructure.ProvideCompanyPurpose(companyPurpose);

                return Task.CompletedTask;
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
