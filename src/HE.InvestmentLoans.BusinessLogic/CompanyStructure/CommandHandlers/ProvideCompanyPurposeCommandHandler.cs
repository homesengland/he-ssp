using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideCompanyPurposeCommandHandler : CompanyStructureBaseCommandHandler, IRequestHandler<ProvideCompanyPurposeCommand, OperationResult>
{
    public ProvideCompanyPurposeCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext, ILogger<CompanyStructureBaseCommandHandler> logger)
        : base(repository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideCompanyPurposeCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            (companyStructure, userAccount) =>
            {
                var companyPurpose = request.CompanyPurpose.IsProvided() ? CompanyPurpose.FromString(request.CompanyPurpose!) : null;
                companyStructure.ProvideCompanyPurpose(companyPurpose);

                return Task.CompletedTask;
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
