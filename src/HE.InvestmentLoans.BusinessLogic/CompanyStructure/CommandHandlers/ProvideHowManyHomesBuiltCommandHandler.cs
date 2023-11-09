using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideHowManyHomesBuiltCommandHandler :
    CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideHowManyHomesBuiltCommand, OperationResult>
{
    public ProvideHowManyHomesBuiltCommandHandler(ICompanyStructureRepository companyStructureRepository, ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext, ILogger<CompanyStructureBaseCommandHandler> logger)
        : base(companyStructureRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideHowManyHomesBuiltCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            companyStructure =>
            {
                var homesBuild = request.HomesBuilt.IsProvided() ? HomesBuilt.FromString(request.HomesBuilt!) : null;
                companyStructure.ProvideHowManyHomesBuilt(homesBuild);

                return Task.CompletedTask;
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
